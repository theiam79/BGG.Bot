using AutoMapper;
using AutoMapper.QueryableExtensions;
using BGG.Bot.Core.Models;
using BGG.Bot.Core.Models.Collection;
using BGG.Bot.Data.Context;
using BGG.Bot.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.Core.Services
{
  public class CollectionService
  {
    private readonly CollectionContext _collectionContext;
    private readonly BggQueryService _bgg;
    private readonly IMapper _mapper;
    private readonly ILogger<CollectionService> _logger;

    public CollectionService(CollectionContext collectionContext, ILogger<CollectionService> logger, BggQueryService bgg, IMapper mapper)
    {
      _collectionContext = collectionContext;
      _logger = logger;
      _bgg = bgg;
      _mapper = mapper;
    }

    public async Task<CommandResult> Register(ulong discordId, string bggUsername)
    {
      if (await _collectionContext.Users.AnyAsync(u => u.BggUsername == bggUsername))
      {
        return new CommandResult(false, $"{bggUsername} has already been registered");
      }

      if (!await _bgg.ValidUser(bggUsername))
      {
        return new CommandResult(false, $"{bggUsername} is not a valid BGG username");
      }

      var newUser = new User { DiscordId = discordId, BggUsername = bggUsername };
      var user = await _collectionContext.Users.AddAsync(newUser);
      await _collectionContext.SaveChangesAsync();

      return await UpdateCollection(discordId, bggUsername);
    }

    public async Task<CommandResult> Unregister(ulong discordId, string bggUsername)
    {
      var registeredCollection = await _collectionContext.Users.FirstOrDefaultAsync(u => u.DiscordId == discordId && u.BggUsername == bggUsername);
      if (registeredCollection == null)
      {
        return new CommandResult(false, $"{bggUsername} is not currently registered");
      }

      _collectionContext.Remove(registeredCollection);
      await _collectionContext.SaveChangesAsync();
      return new CommandResult(true, $"Successfully unregistered {bggUsername}");
    }

    public async Task<CommandResult> UpdateCollection(ulong discordId, string bggUsername)
    {
      var registeredUser = await _collectionContext.Users.Include(u => u.UserCollectionItems).FirstOrDefaultAsync(u => u.DiscordId == discordId && u.BggUsername == bggUsername);
      if (registeredUser == null)
      {
        return new CommandResult(false, $"{bggUsername} is not currently registered");
      }
      return await UpdateCollection(registeredUser);
    }

    public async Task<CommandResult> UpdateCollection(User registeredUser)
    {
      var bggCollection = await _bgg.GetBggCollectionAsync(registeredUser.BggUsername);

      await AddMissingGames(bggCollection.BggCollectionItems);

      var userCollectionItems = _mapper.Map<List<UserCollectionItem>>(bggCollection.BggCollectionItems.Unique());

      _collectionContext.UserCollectionItem.RemoveRange(registeredUser.UserCollectionItems);

      registeredUser.UserCollectionItems = userCollectionItems;
      _collectionContext.Users.Update(registeredUser);
      await _collectionContext.SaveChangesAsync();

      return new CommandResult(true, $"Successfully updated collection for {registeredUser.BggUsername} - {registeredUser.UserCollectionItems.Count} total games");
    }

    public async Task<CommandResult> UpdatePlayedGames(ulong discordId, string bggUsername)
    {
      var registeredUser = await _collectionContext.Users.Include(u => u.UserCollectionItems).FirstOrDefaultAsync(u => u.DiscordId == discordId && u.BggUsername == bggUsername);
      if (registeredUser == null)
      {
        return new CommandResult(false, $"{bggUsername} is not currently registered");
      }
      return await UpdatePlayedGames(registeredUser);
    }

    public async Task<CommandResult> UpdatePlayedGames(User registeredUser)
    {
      var playedGames = await _bgg.GetBggCollectionAsync(registeredUser.BggUsername, played: true);
      await AddMissingGames(playedGames.BggCollectionItems);

      var userPlayedItems = _mapper.Map<List<UserPlayedItem>>(playedGames.BggCollectionItems.Unique());

      _collectionContext.UserPlayedItems.RemoveRange(registeredUser.UserPlayedItems ?? Enumerable.Empty<UserPlayedItem>());
      registeredUser.UserPlayedItems = userPlayedItems;
      _collectionContext.Users.Update(registeredUser);

      await _collectionContext.SaveChangesAsync();
      return new CommandResult(true, $"Successfully updated plays for {registeredUser.BggUsername} - {registeredUser.UserCollectionItems.Count} total games");
    }

    public async Task<List<User>> GetCollections()
    {
      return await _collectionContext.Users.OrderBy(u => u.BggUsername).ToListAsync();
    }

    public async Task AddMissingGames(IEnumerable<BggCollectionItem> bggItems)
    {
      var existingItems = await _collectionContext.Items.Select(ci => ci.BggId).ToListAsync();
      var newItems = bggItems.Unique().Where(ci => !existingItems.Any(e => e == ci.BggId));

      await _collectionContext.Items.AddRangeAsync(newItems.Select(ci => new Item { BggId = ci.BggId, Name = ci.Name }));
      await _collectionContext.SaveChangesAsync();
    }

    public async Task<List<User>> FindOwners(int bggId)
    {
     return await _collectionContext.Items
        .Include(ci => ci.UserCollectionItems)
        .ThenInclude(uci => uci.User)
        .Where(ci => ci.BggId == bggId)
        .SelectMany(ci => ci.UserCollectionItems.Where(uci => uci.Owned).Select(uci => uci.User))
        .ToListAsync();
    }

    public async Task<List<User>> FindPlayers(int bggId)
    {
      return await _collectionContext.Items
         .Include(ci => ci.UserCollectionItems)
         .ThenInclude(uci => uci.User)
         .Where(ci => ci.BggId == bggId)
         .SelectMany(ci => ci.UserCollectionItems.Where(uci => uci.WantToPlay || uci.Rating >= 7).Select(uci => uci.User))
         .ToListAsync();
    }

    public async Task<List<User>> FindPlayed(int bggId)
    {
      return await _collectionContext.Items
        .Include(i => i.UserPlayedItems)
        .ThenInclude(upi => upi.User)
        .Where(i => i.BggId == bggId)
        .SelectMany(i => i.UserPlayedItems.Select(upi => upi.User))
        .ToListAsync();
    }
  }

  public static class BggCollectionExtensions
  {
    public static IEnumerable<BggCollectionItem> Unique(this IEnumerable<BggCollectionItem> items) => items.GroupBy(i => i.BggId).Select(g => g.First());
  }
}

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

      var bggCollection = await _bgg.GetBggCollectionAsync(bggUsername);

      await AddMissingGames(bggCollection.BggCollectionItems);

      var userCollectionItems = _mapper.Map<List<UserCollectionItem>>(bggCollection.BggCollectionItems.Unique());
      var newUser = new User { DiscordId = discordId, BggUsername = bggUsername, UserCollectionItems = userCollectionItems };
      await _collectionContext.Users.AddAsync(newUser);
      await _collectionContext.SaveChangesAsync();

      return new CommandResult(true, $"Successfully registered {bggUsername} - collection contained {bggCollection.TotalItems} games");
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

    public async Task AddMissingGames(IEnumerable<BggCollectionItem> bggItems)
    {
      var existingItems = await _collectionContext.CollectionItems.Select(ci => ci.BggId).ToListAsync();
      var newItems = bggItems.Unique().Where(ci => !existingItems.Any(e => e == ci.BggId));

      await _collectionContext.CollectionItems.AddRangeAsync(newItems.Select(ci => new CollectionItem { BggId = ci.BggId, Name = ci.Name }));
      await _collectionContext.SaveChangesAsync();
    }

    public async Task<List<User>> FindOwners(int bggId)
    {
     return await _collectionContext.CollectionItems
        .Include(ci => ci.UserCollectionItems)
        .ThenInclude(uci => uci.User)
        .Where(ci => ci.BggId == bggId)
        .SelectMany(ci => ci.UserCollectionItems.Where(uci => uci.Owned).Select(uci => uci.User))
        .ToListAsync();
    }

    public async Task<List<User>> FindPlayers(int bggId)
    {
      return await _collectionContext.CollectionItems
         .Include(ci => ci.UserCollectionItems)
         .ThenInclude(uci => uci.User)
         .Where(ci => ci.BggId == bggId)
         .SelectMany(ci => ci.UserCollectionItems.Where(uci => uci.WantToPlay).Select(uci => uci.User))
         .ToListAsync();
    }
  }

  public static class BggCollectionExtensions
  {
    public static IEnumerable<BggCollectionItem> Unique(this IEnumerable<BggCollectionItem> items) => items.GroupBy(i => i.BggId).Select(g => g.First());
  }
}

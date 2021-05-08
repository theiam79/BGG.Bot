using BGG.Bot.Core.Models;
using BGG.Bot.Data.Context;
using BGG.Bot.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.Core.Services
{
  public class RegistrationService
  {
    private readonly CollectionContext _collectionContext;
    private readonly BggQueryService _bgg;

    public RegistrationService(CollectionContext collectionContext, BggQueryService bgg)
    {
      _collectionContext = collectionContext;
      _bgg = bgg;
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

      return new CommandResult(true);
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

    public async Task<List<User>> GetRegistrations()
    {
      return await _collectionContext.Users
        .OrderBy(u => u.BggUsername)
        .ToListAsync();
    }
  }
}

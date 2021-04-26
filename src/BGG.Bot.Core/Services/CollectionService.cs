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
    private readonly HttpClient _httpClient;
    private readonly BggQueryService _bgg;
    private readonly ILogger<CollectionService> _logger;

    public CollectionService(CollectionContext collectionContext, ILogger<CollectionService> logger, HttpClient httpClient, BggQueryService bgg)
    {
      _collectionContext = collectionContext;
      _logger = logger;
      _httpClient = httpClient;
      _bgg = bgg;
    }

    public async Task<bool> Register(ulong discordId, string bggUsername)
    {
      if (!await _bgg.ValidUser(bggUsername))
      {
        return false;
      }

      var bggCollection = await _bgg.GetBggCollectionAsync(bggUsername);

      var existing = await _collectionContext.CollectionItems.Select(ci => ci.BggId).ToListAsync();
      var uniqueCI = bggCollection.CollectionItems.GroupBy(ci => ci.BggId).SelectMany(g => g.Take(1)).Where(ci => !existing.Any(e => e == ci.BggId));

      await _collectionContext.CollectionItems.AddRangeAsync(uniqueCI.Select(ci => new CollectionItem { BggId = ci.BggId, Name = ci.Name }));
      await _collectionContext.SaveChangesAsync();

      var adf = bggCollection.CollectionItems.GroupBy(ci => ci.BggId).Select(x => x.Key).ToList();

      //var uci = await _collectionContext.CollectionItems.Where(ci => adf.Contains(ci.BggId)).ToListAsync();


      //var userData = new User { DiscordId = discordId, BggUsername = bggUsername, UserCollectionItems = uci.Select(x => new UserCollectionItem { CollectionItemId = x.CollectionItemId }).ToList() };

      var newUser = new User { DiscordId = discordId, BggUsername = bggUsername, UserCollectionItems = bggCollection.CollectionItems.GroupBy(ci => ci.BggId).Select(g => new UserCollectionItem { BggId = g.Key }).ToList() };



      await _collectionContext.Users.AddAsync(newUser);
      await _collectionContext.SaveChangesAsync();









      return true;
    }
  }
}

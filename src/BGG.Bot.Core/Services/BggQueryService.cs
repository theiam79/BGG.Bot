using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BGG.Bot.Core.Models.Search;
using Microsoft.Extensions.Logging;
using Polly.Registry;
using BGG.Bot.Core.Models.Thing;
using BGG.Bot.Core.Models.User;
using BGG.Bot.Core.Models.Collection;

namespace BGG.Bot.Core.Services
{
  public class BggQueryService
  {
    private readonly ILogger<BggQueryService> _logger;
    private readonly HttpClient _httpClient;

    public BggQueryService(ILogger<BggQueryService> logger, HttpClient httpClient)
    {
      _logger = logger;
      _httpClient = httpClient;
    }

    public async Task<IEnumerable<Item>> Search(string searchTerm, bool exact = false)
    {
      var queryParams = new Dictionary<string, string>()
      {
        {"query", searchTerm },
        {"type", "boardgame" },
        {"exact", exact ? "1" : "0" }
      };

      var url = new Uri(QueryHelpers.AddQueryString(@"https://www.boardgamegeek.com/xmlapi2/search", queryParams));

      var response = await _httpClient.GetStreamAsync(url);
      var serializer = new XmlSerializer(typeof(SearchResult));

      var results = serializer.Deserialize(response) as SearchResult;

      return results.Items.ToList();
    }

    public async Task<Thing> GetThing(string id)
    {
      var queryParams = new Dictionary<string, string>() { { "id", id } };
      var url = new Uri(QueryHelpers.AddQueryString(@"https://www.boardgamegeek.com/xmlapi2/thing", queryParams));

      var response = await _httpClient.GetStreamAsync(url);
      var serializer = new XmlSerializer(typeof(ThingResult));

      var results = serializer.Deserialize(response) as ThingResult;
      return results.Things?[0] ?? null;
    }

    public async Task<BggCollection> GetBggCollectionAsync(string username)
    {
      var queryParams = new Dictionary<string, string>() { { "username", username }, { "excludesubtype","boardgameexpansion" } };
      var url = new Uri(QueryHelpers.AddQueryString(@"https://www.boardgamegeek.com/xmlapi2/collection", queryParams));

      var response = await _httpClient.GetStreamAsync(url);
      var serializer = new XmlSerializer(typeof(BggCollection));

      var result = serializer.Deserialize(response) as BggCollection;
      return result;
    }

    public async Task<bool> ValidUser(string username)
    {
      var queryParams = new Dictionary<string, string>() { { "name", username } };
      var url = new Uri(QueryHelpers.AddQueryString(@"https://www.boardgamegeek.com/xmlapi2/user", queryParams));

      var response = await _httpClient.GetStreamAsync(url);
      var serializer = new XmlSerializer(typeof(BggUser));

      var result = serializer.Deserialize(response) as BggUser;
      return !string.IsNullOrEmpty(result.Id);
    }
  }
}

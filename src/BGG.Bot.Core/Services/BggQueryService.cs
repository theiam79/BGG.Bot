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



    public async Task<Thing> GetThing(int id)
    {
      var queryParams = new Dictionary<string, string>() { { "id", id.ToString() } };
      var url = new Uri(QueryHelpers.AddQueryString(@"https://www.boardgamegeek.com/xmlapi2/thing", queryParams));

      var response = await _httpClient.GetStreamAsync(url);
      var serializer = new XmlSerializer(typeof(ThingResult));

      var results = serializer.Deserialize(response) as ThingResult;
      return results.Things?[0] ?? null;
    }

    public async Task<BggCollection> GetBggCollectionAsync(string username, bool? own = null, bool? rated = null, bool? played = null, bool? comment = null, bool? trade = null,
      bool? want = null, bool? wishlist = null, bool? preordered = null, bool? wantToPlay = null, bool? wantToBuy = null, bool? prevOwned = null)
    {
      var queryParams = new Dictionary<string, string>()
      {
        { "username", username }, 
        { "stats", "1" },
      };

      if (own != null) queryParams.Add("own", (bool)own ? "1" : "0");
      if (rated != null) queryParams.Add("rated", (bool)rated ? "1" : "0");
      if (played != null) queryParams.Add("played", (bool)played ? "1" : "0");
      if (comment != null) queryParams.Add("comment", (bool)comment ? "1" : "0" );
      if (trade != null) queryParams.Add("trade", (bool)trade ? "1" : "0");
      if (want != null) queryParams.Add("want", (bool)want ? "1" : "0");
      if (wishlist != null) queryParams.Add("wishlist", (bool)wishlist ? "1" : "0");
      if (preordered != null) queryParams.Add("preordered", (bool)preordered ? "1" : "0");
      if (wantToPlay != null) queryParams.Add("wanttoplay", (bool)wantToPlay ? "1" : "0");
      if (wantToBuy != null) queryParams.Add("wanttobuy", (bool)wantToBuy ? "1" : "0");
      if (prevOwned != null) queryParams.Add("prevowned", (bool)prevOwned ? "1" : "0");

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

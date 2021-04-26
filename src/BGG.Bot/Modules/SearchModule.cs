using BGG.Bot.Core.Services;
using BGG.Bot.ResultSelection;
using Discord;
using Discord.Commands;
using MoreLinq.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BGG.Bot.Modules
{
  public class SearchModule : SelectionBase<SocketCommandContext>
  {
    private readonly BggQueryService _bgg;

    public SearchModule(BggQueryService bgg)
    {
      _bgg = bgg;
    }

    [Command("search")]
    public async Task Search([Remainder] string searchTerm)
    {
      var results = await _bgg.Search(searchTerm);
      var selection = await SelectResult(results.ToIndexedResults(), true, ir => $"{ir.Index}) {ir.Result.Name.Value}");

      if (selection != null)
      {
        var thing = await _bgg.GetThing(selection.Result.Id);

        if (thing == null)
        {
          await ReplyAsync($"Something went wrong, unable to find BGG thing {selection.Result.Id}");
          return;
        }

        var embed = new EmbedBuilder()
          .WithDescription(HttpUtility.HtmlDecode(thing.Description).Split('\n', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "No description")
          .WithImageUrl(thing.Image)
          .WithThumbnailUrl(thing.Thumbnail)
          .WithTitle(thing.Name.Value)
          .WithUrl($"https://boardgamegeek.com/boardgame/{thing.Id}")
          .Build();

        await ReplyAsync(embed: embed);
      }
      else
      {
        await ReplyAsync("No valid selection made before timeout");
      }
    }
  }
}

using BGG.Bot.Core.Services;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using MoreLinq.Extensions;
using BGG.Bot.Core.Models.Search;
using Discord;
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

  public class SelectionBase<TContext> : InteractiveBase<TContext> where TContext: SocketCommandContext
  {
    public async Task<IndexedResult<TResult>> SelectResult<TResult>(List<IndexedResult<TResult>> indexedResults, bool cleanupMessages) => await SelectResult(indexedResults, cleanupMessages, ir => $"{ir.Index}) {ir.Result}");
    public async Task<IndexedResult<TResult>> SelectResult<TResult>(List<IndexedResult<TResult>> indexedResults, bool cleanupMessages, Func<IndexedResult<TResult>, string> displayTextGenerator)
    {
      var pages = indexedResults.Batch(10).Select(b => string.Join(Environment.NewLine, b.Select(ir => displayTextGenerator(ir))));

      var message = new PaginatedMessage
      {
        Pages = pages,
        Content = $"Please select a result:",
      };

      var messagesToDelete = new List<IDeletable>();
      messagesToDelete.Add(await PagedReplyAsync(message));

      var selectionMade = false;
      var selectionCancelled = false;
      int selection = 0;

      while (!(selectionMade || selectionCancelled))
      {
        var reply = await NextMessageAsync();
        if (reply != null)
        {
          messagesToDelete.Add(reply);
          selectionMade = int.TryParse(reply.Content, out selection) && selection <= indexedResults.Count;
          if (!selectionMade)
          {
            messagesToDelete.Add(await ReplyAsync("You must enter a valid number contained in the results"));
          }
        }
        else
        {
          selectionCancelled = true;
        }
      }
      if (cleanupMessages) messagesToDelete.ForEach(mtd => mtd.DeleteAsync());
      return selectionMade ? indexedResults.FirstOrDefault(ir => ir.Index == selection) : null;
    }
  }

  public class IndexedResult <T>
  {
    public int Index { get; set; }
    public T Result { get; set; }
    public IndexedResult(int index, T result)
    {
      Index = index;
      Result = result;
    }
  }

  public static class IndexedResultHelpers
  {
    public static List<IndexedResult<T>> ToIndexedResults<T>(this IEnumerable<T> results)
    {
      var i = 1;
      return results.Select(r => new IndexedResult<T>(i++, r)).ToList();
    }
  }
}

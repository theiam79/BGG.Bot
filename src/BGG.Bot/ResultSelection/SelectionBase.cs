using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.ResultSelection
{
  public class SelectionBase<TContext> : InteractiveBase<TContext> where TContext : SocketCommandContext
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
        var reply = await NextMessageAsync(timeout: TimeSpan.FromSeconds(60));
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
}

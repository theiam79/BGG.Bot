﻿using BGG.Bot.Core.Services;
using BGG.Bot.ResultSelection;
using Discord;
using Discord.Commands;
using MoreLinq.Extensions;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BGG.Bot.Modules
{
  [Group("search")]
  public class SearchModule : SelectionBase<SocketCommandContext>
  {
    private readonly BggQueryService _bgg;
    private readonly CollectionService _collectionService;

    public SearchModule(BggQueryService bgg, CollectionService collectionService)
    {
      _bgg = bgg;
      _collectionService = collectionService;
    }

    [Command("game")]
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

    [Command("owner")]
    [Summary("Find any players that own the specified game")]
    public async Task Owns([Remainder] string searchTerm)
    {
      var results = await _bgg.Search(searchTerm);
      var selection = await SelectResult(results.ToIndexedResults(), true, ir => $"{ir.Index}) {ir.Result.Name.Value}");

      var result = await _collectionService.FindOwners(selection.Result.Id);

      var message = "Found no registered owners";

      if (result.Any())
      { 
        message = new StringBuilder()
          .AppendLine($"Found {result.Count} registered owners:")
          .AppendJoin(Environment.NewLine, result.Select(r => $"<@{r.DiscordId}>"))
          .ToString();
      }

      await ReplyAsync(message);
    }

    [Command("player")]
    [Summary("Find any players that want to play the specified game")]
    public async Task Plays([Remainder] string searchTerm)
    {
      var results = await _bgg.Search(searchTerm);
      var selection = await SelectResult(results.ToIndexedResults(), true, ir => $"{ir.Index}) {ir.Result.Name.Value}");

      var result = await _collectionService.FindPlayers(selection.Result.Id);

      var message = "Found no registered owners";

      if (result.Any())
      {
        message = new StringBuilder()
          .AppendLine($"Found {result.Count} people interested in playing:")
          .AppendJoin(Environment.NewLine, result.Select(r => $"<@{r.DiscordId}>"))
          .ToString();
      }

      await ReplyAsync(message);
    }
  }
}

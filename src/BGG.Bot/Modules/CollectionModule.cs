using BGG.Bot.Core.Models;
using BGG.Bot.Core.Services;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.Modules
{
  [Group("collection")]
  public class CollectionModule : ModuleBase<SocketCommandContext>
  {
    private readonly CollectionService _collectionService;

    public CollectionModule(CollectionService collectionService)
    {
      _collectionService = collectionService;
    }

    [Command("register")]
    [Summary("Register your collection to enable bot functionality")]
    public async Task Register([Remainder] string bggUsername)
    {
      var result = await _collectionService.Register(Context.User.Id, bggUsername);
      await Respond(result);
    }

    [Command("Unregister")]
    [Summary("Unregister your collection to disable bot functionality")]
    public async Task Unregister([Remainder] string bggUsername)
    {
      var result = await _collectionService.Unregister(Context.User.Id, bggUsername);
      await Respond(result);
    }

    [Command("update")]
    [Summary("Forces an update of your collection items")]
    public async Task Update([Remainder] string bggUsername)
    {
      var result = await _collectionService.UpdateCollection(Context.User.Id, bggUsername);
      await Respond(result);
    }

    async Task Respond(CommandResult result)
    {
      var responseMessage = result.Success ? result.Message : $"Failed to register collection: {result.Message}";
      if (!string.IsNullOrEmpty(responseMessage))
      {
        await ReplyAsync(responseMessage);
      }
    }
  }
}

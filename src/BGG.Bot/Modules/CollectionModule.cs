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
      await _collectionService.Register(Context.User.Id, bggUsername);
    }
  }
}

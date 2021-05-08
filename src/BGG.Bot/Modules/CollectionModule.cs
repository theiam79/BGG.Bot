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
    private readonly RegistrationService _registrationService;

    public CollectionModule(CollectionService collectionService, RegistrationService registrationService)
    {
      _collectionService = collectionService;
      _registrationService = registrationService;
    }

    [Command("force-register")]
    [Summary("Force register a collection for a specified user")]
    [RequireOwner]
    public async Task ForceRegister(ulong discordId, [Remainder] string bggUsername)
    {
      var result = await _registrationService.Register(discordId, bggUsername);
      if (result.Success)
      {
        result = await _collectionService.UpdateCollection(discordId, bggUsername);
      }
      if (result.Success)
      {
        result = await _collectionService.UpdatePlayedGames(discordId, bggUsername);
      }
      await Respond(result);
    }

    [Command("register")]
    [Summary("Register your collection to enable bot functionality")]
    public async Task Register([Remainder] string bggUsername)
    {
      var result = await _registrationService.Register(Context.User.Id, bggUsername);
      if (result.Success)
      {
        result = await _collectionService.UpdateCollection(Context.User.Id, bggUsername);
      }
      if (result.Success)
      {
        result = await _collectionService.UpdatePlayedGames(Context.User.Id, bggUsername);
      }
      await Respond(result);
    }

    [Command("force-unregister")]
    [Summary("Force unregister a collection for a specified user")]
    [RequireOwner]
    public async Task ForceUnregister(ulong discordId, [Remainder] string bggUsername)
    {
      var result = await _registrationService.Unregister(discordId, bggUsername);
      await Respond(result);
    }

    [Command("unregister")]
    [Summary("Unregister your collection to disable bot functionality")]
    public async Task Unregister([Remainder] string bggUsername)
    {
      var result = await _registrationService.Unregister(Context.User.Id, bggUsername);
      await Respond(result);
    }

    [Command("update")]
    [Summary("Forces an update of your collection and played list")]
    public async Task Update([Remainder] string bggUsername)
    {
      var result = await _collectionService.UpdateCollection(Context.User.Id, bggUsername);
      if (result.Success)
      {
        result = await _collectionService.UpdatePlayedGames(Context.User.Id, bggUsername);
      }
      await Respond(result);
    }

    [Command("update-all")]
    [Summary("Update all collections and played lists")]
    [RequireOwner]
    public async Task UpdateAll(bool verbose = false)
    {
      var collections = await _collectionService.GetCollections();
      await ReplyAsync($"Found {collections.Count} to update");

      var results = new List<CommandResult>();
      foreach (var user in collections)
      {
        results.Add(await _collectionService.UpdateCollection(user));
        results.Add(await _collectionService.UpdatePlayedGames(user));
        if (verbose)
        {
          await ReplyAsync($"Updated {user.BggUsername}");
        }
      }

      await ReplyAsync($"Finished updating all collections with {results.Count(r => !r.Success)} errors");
    }

    [Command("list")]
    [Summary("List all registered collections")]
    public async Task ListCollections()
    {
      var users = await _registrationService.GetRegistrations();
      var sb = new StringBuilder()
        .AppendLine($"Found {users.Count} registered collections")
        .AppendJoin(Environment.NewLine, users.Select(u => $"<@{u.DiscordId}> - {u.BggUsername}"));
      await ReplyAsync(sb.ToString());
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

using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BGG.Bot.Services
{
  public class CommandHandler : InitializedService
  {
    private readonly IServiceProvider _services;
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commandService;

    public CommandHandler(IServiceProvider services, DiscordSocketClient client, CommandService commandService)
    {
      _services = services;
      _client = client;
      _commandService = commandService;

      _client.MessageReceived += HandleCommandAsync;
      _commandService.CommandExecuted += CommandExecutedAsync;
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken)
    {
      await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
    }

    private async Task HandleCommandAsync(SocketMessage incomingMessage)
    {
      if (!(incomingMessage is SocketUserMessage message)) return;
      if (message.Source != MessageSource.User) return;

      int argPos = 0;
      if (!(incomingMessage.Channel is SocketDMChannel))
      {
        if (!message.HasStringPrefix("$", ref argPos) && !message.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;
      }

      var context = new SocketCommandContext(_client, message);
      await _commandService.ExecuteAsync(context, argPos, _services);
    }

    public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
    {
      if (!command.IsSpecified && result.IsSuccess)
        return;

      switch (result.Error)
      {
        case CommandError.UnknownCommand:
          break;
        case CommandError.BadArgCount:
          await context.Channel.SendMessageAsync("Incorrect command usage, showing helper:");
          //EmbedBuilder builder = _helper.GetHelpInformationBuilder(command.Value);
          //await context.Channel.SendMessageAsync(embed: builder.Build());
          break;
        case CommandError.ParseFailed:
        case CommandError.ObjectNotFound:
        case CommandError.MultipleMatches:
        case CommandError.UnmetPrecondition:
          await context.Channel.SendMessageAsync(result.ErrorReason);
          break;
        case CommandError.Exception:
          //await context.Channel.SendMessageAsync($"An error occurred whilst processing this command. This has been reported automatically. (ID: {SentrySdk.LastEventId})");
          break;
        case CommandError.Unsuccessful:
          break;
        case null:
          break;
      }
    }
  }
}

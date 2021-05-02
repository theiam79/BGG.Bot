using BGG.Bot.Models;
using Discord;
using Discord.Commands;
using Discord.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.Services
{
  public class HelperService
  {
    private readonly CommandService _commandService;
    private readonly IServiceProvider _serviceProvider;
    private readonly BotOptions _botOptions;

    public HelperService(CommandService commandService, IServiceProvider serviceProvider, IOptions<BotOptions> botOptions)
    {
      _commandService = commandService;
      _serviceProvider = serviceProvider;
      _botOptions = botOptions.Value;
    }

    public async Task HelpAsync(ICommandContext context)
    {
      string prefix = _botOptions.CommandPrefix;
      var builder = new EmbedBuilder
      {
        Color = new Color(114, 137, 218),
        Description = $"These are the commands you have access to on the guild **{context.Guild.Name}**.\nAvailable commands differ based on your guild permissions.\nFor more information, use {prefix}help [command]"
      };

      foreach (var module in _commandService.Modules)
      {
        if (module.Name == "HelpModule")
          continue;
        StringBuilder description = new StringBuilder();
        foreach (var cmd in module.Commands)
        {
          var result = await cmd.CheckPreconditionsAsync(context, _serviceProvider);
          if (result.IsSuccess)
          {
            description.Append($"{prefix}{cmd.Aliases.First()} ");
            if (cmd.Parameters.Any())
              description.Append($"[{string.Join(", ", cmd.Parameters.Select(p => p.Name))}]");
            description.Append("\n");
          }
        }
        if (!string.IsNullOrWhiteSpace(description.ToString()))
        {
          builder.AddField(x =>
          {
            x.Name = module.Name;
            x.Value = description.ToString();
            x.IsInline = false;
          });
        }
      }

      try
      {
        var DMChannel = await context.User.GetOrCreateDMChannelAsync();
        await DMChannel.SendMessageAsync(embed: builder.Build());
      }
      catch (HttpException)
      {
        await context.Channel.SendMessageAsync(
            "Error: You have PMs disabled on this server. Please enable direct messages in your privacy settings and try again.");
      }
    }

    public async Task HelpAsync(ICommandContext context, string command)
    {
      var result = _commandService.Search(context, command);

      if (!result.IsSuccess)
      {
        await context.Channel.SendMessageAsync($"Sorry, I couldn't find a command like **{command}**.");
        return;
      }
      var builder = new EmbedBuilder
      {
        Color = new Color(114, 137, 218),
        Description = $"Here are some commands like **{command}**"

      };

      foreach (var match in result.Commands)
      {
        var cmd = match.Command;
        var param = cmd.Parameters.Select(p => p.Name);
        builder.AddField(x =>
        {
          x.Name = string.Join(", ", cmd.Aliases);
          x.Value = $"Parameters: {(param.Any() ? string.Join(", ", param) : "None")}\n" +
                    $"Summary: {cmd.Summary}";
          x.IsInline = false;
        });
      }

      await context.Channel.SendMessageAsync(embed: builder.Build());
    }

    public EmbedBuilder GetHelpInformationBuilder(CommandInfo info)
    {
      var builder = new EmbedBuilder().WithColor(114, 137, 218);

      var param = info.Parameters.Select(p => p.Name);
      builder.AddField(x =>
      {
        x.Name = string.Join(", ", info.Aliases);
        x.Value = $"Parameters: {(param.Any() ? string.Join(", ", param) : "None")}\n" +
                  $"Summary: {info.Summary}";
        x.IsInline = false;
      });

      return builder;
    }
  }
}

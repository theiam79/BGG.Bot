using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using Discord.Addons.Interactive;
using BGG.Bot.Services;
using BGG.Bot.Core.Extensions;

namespace BGG.Bot
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureDiscordHost<DiscordSocketClient>((context, config) =>
            {
              config.SocketConfig = new DiscordSocketConfig
              {
                LogLevel = Discord.LogSeverity.Info,
                AlwaysDownloadUsers = true,
                MessageCacheSize = 200
              };

              config.Token = context.Configuration["token"];
            })
            .UseCommandService((context, config) =>
            {
              config.LogLevel = Discord.LogSeverity.Verbose;
              config.DefaultRunMode = Discord.Commands.RunMode.Async;
            })
            .ConfigureServices((hostContext, services) =>
            {
              services
                .AddHostedService<CommandHandler>()
                .AddSingleton<InteractiveService>()
                .AddBgg();
            });
  }
}

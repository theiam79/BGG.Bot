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
using BGG.Bot.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BGG.Bot.Models;
using Serilog;
using Serilog.Events;
using Octokit;
using System.Reflection;

namespace BGG.Bot
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();
      using (host)
      {
        await host.MigrateDatabase<CollectionContext>().RunAsync();
      }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog((context,config) =>
            {
              config
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error);

              if (!string.IsNullOrEmpty(context.Configuration["sentry"]))
              {
                config.WriteTo.Sentry(o =>
                {
                  o.Dsn = context.Configuration["sentry"];
                  o.MinimumBreadcrumbLevel = LogEventLevel.Debug;
                  o.MinimumEventLevel = LogEventLevel.Error;
                });
              }
            })
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
                .Configure<BotOptions>(hostContext.Configuration.GetSection("BotOptions"))
                .AddHostedService<CommandHandler>()
                .AddSingleton<InteractiveService>()
                .AddSingleton<HelperService>()
                .AddTransient<IGitHubClient>(s => new GitHubClient(new ProductHeaderValue(Assembly.GetEntryAssembly().GetName().Name)) { Credentials = new Credentials(hostContext.Configuration["github_token"]) })
                .AddDbContext<CollectionContext>(options =>
                {
                  options.UseSqlite(hostContext.Configuration.GetConnectionString("CollectionDB"));
                }, ServiceLifetime.Transient, ServiceLifetime.Transient)
                .AddBgg();
            });
  }

  public static class HostExtensions
  {
    public static IHost MigrateDatabase<T>(this IHost host) where T : DbContext
    {
      using (var scope = host.Services.CreateScope())
      {
        var services = scope.ServiceProvider;
        try
        {
          var db = services.GetRequiredService<T>();
          db.Database.Migrate();
        }
        catch (Exception ex)
        {
          var logger = services.GetRequiredService<ILogger<Program>>();
          logger.LogError(ex, "An error occurred while migrating the database.");
        }
      }
      return host;
    }
  }
}

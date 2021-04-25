using BGG.Bot.Core.Extensions;
using BGG.Bot.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Test
{
  class Program
  {
    static async Task Main(string[] args)
    {
      await CreateHostBuilder(args).Build().Services.GetRequiredService<App>().Run(args);
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
          .ConfigureServices((_, services) =>
            services.AddBgg()
              .AddTransient<App>());
  }
}

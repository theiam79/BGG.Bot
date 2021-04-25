using BGG.Bot.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
  public class App
  {
    private readonly BggQueryService _bgg;
    private readonly ILogger<App> _logger;

    public App(BggQueryService bgg, ILogger<App> logger)
    {
      _bgg = bgg;
      _logger = logger;
    }

    public async Task Run(string[] args)
    {
      _logger.LogInformation("Starting app");
      while (true)
      {
        Console.WriteLine("Please enter a search term:");
        var search = Console.ReadLine();
        Console.WriteLine();
        var results = await _bgg.Search(search);
        Console.WriteLine($"Found {results.Count()} results for \"{search}\":");
        foreach (var res in results)
        {
          Console.WriteLine(res.Name.Value);
        }

        Console.WriteLine();
        Console.WriteLine();
      }
    }
  }
}

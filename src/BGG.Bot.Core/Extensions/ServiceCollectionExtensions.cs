using BGG.Bot.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.Core.Extensions
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddBgg(this IServiceCollection services)
    {
      services.AddHttpClient<BggQueryService>()//.AddPolicyHandler()
        ;
      services.AddHttpClient<CollectionService>();
      return services;
    }
  }
}

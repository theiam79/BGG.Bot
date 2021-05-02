using BGG.Bot.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.Core.Extensions
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddBgg(this IServiceCollection services)
    {
      var collectionGenerationPolicy = Policy
        .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.Accepted)
        .WaitAndRetryAsync(10, r => TimeSpan.FromSeconds(Math.Pow(r + 1, 2)));

      HttpStatusCode[] retryableStatusCodes =
      {
        HttpStatusCode.RequestTimeout,
        HttpStatusCode.InternalServerError,
        HttpStatusCode.BadGateway,
        HttpStatusCode.ServiceUnavailable,
        HttpStatusCode.GatewayTimeout
      };

      var genericHttpRetry = Policy
        .HandleResult<HttpResponseMessage>(r => retryableStatusCodes.Contains(r.StatusCode))
        .WaitAndRetryAsync(3, x => TimeSpan.FromSeconds(Math.Pow(3, x)));

      var wrapped = Policy.WrapAsync(collectionGenerationPolicy, genericHttpRetry);

      services
        .AddHttpClient<BggQueryService>()
        .AddPolicyHandler(wrapped);
      
      services.AddTransient<CollectionService>();

      services.AddAutoMapper(System.Reflection.Assembly.GetExecutingAssembly());
      return services;
    }
  }
}

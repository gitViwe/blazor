using System.Net;
using Blazor.Shared.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Blazor.Shared.Extension;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddGatewayClient(this IServiceCollection services)
    {
        services
            .AddHttpClient<IGatewayClient, GatewayClient>(options => options.Timeout = TimeSpan.FromSeconds(60))
            .AddResilienceHandler(GatewayClient.ResilienceHandlerName, resilienceBuilder =>
            {
                resilienceBuilder.AddRetry(new HttpRetryStrategyOptions
                {
                    Delay = TimeSpan.FromSeconds(5),
                    BackoffType = DelayBackoffType.Exponential,
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>()
                        .Handle<TaskCanceledException>()
                        .HandleResult(response => response.StatusCode == HttpStatusCode.GatewayTimeout)
                });
            });
        
        return services;
    }
}
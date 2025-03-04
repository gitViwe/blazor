using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Blazor.Client;

internal class GatewayClient(HttpClient client) : IGatewayClient
{
    public const string ResilienceHandlerName = "GatewayClientResilienceHandler";
    public HttpClient HttpClient => client;
}

internal static class GatewayClientExtensions
{
    internal static IServiceCollection AddGatewayClient(this IServiceCollection services)
    {
        services
            .AddHttpClient<IGatewayClient, GatewayClient>(options => options.Timeout = TimeSpan.FromSeconds(25))
            .AddResilienceHandler(GatewayClient.ResilienceHandlerName, resilienceBuilder =>
            {
                // Retry Strategy configuration
                resilienceBuilder.AddRetry(new HttpRetryStrategyOptions // Configures retry behavior
                {
                    Delay = TimeSpan.FromSeconds(5),
                    BackoffType = DelayBackoffType.Exponential,
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>()
                });
            });
        
        return services;
    }
}
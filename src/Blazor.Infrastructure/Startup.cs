using Blazor.Infrastructure.Client;

namespace Blazor.Infrastructure;

public static class Startup
{
    public static IServiceCollection RegisterClientAuthorization(this IServiceCollection services)
    {
        services.AddOptions();
        services.AddAuthorizationCore();

        services.AddScoped<HubAuthenticationStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<HubAuthenticationStateProvider>());

        return services;
    }
    
    public static IServiceCollection AddGatewayClient(this IServiceCollection services)
    {
        services
            .AddHttpClient<IGatewayClient, GatewayClient>(options => options.Timeout = TimeSpan.FromSeconds(60))
            .AddHttpMessageHandler<AuthenticationHeaderMessageHandler>()
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
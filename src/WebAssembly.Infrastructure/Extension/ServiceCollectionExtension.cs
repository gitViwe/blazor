using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using WebAssembly.Infrastructure.Authentication;
using WebAssembly.Infrastructure.Manager;
using WebAssembly.Infrastructure.Service;

namespace WebAssembly.Infrastructure.Extension;

internal static class ServiceCollectionExtension
{
    /// <summary>
    /// Registers the services required by the application
    /// </summary>
    internal static IServiceCollection RegisterClientServices(this IServiceCollection services)
    {
        services.AddSingleton<IStorageService, LocalStorageService>();
        services.AddTransient<IInterceptorService, InterceptorService>();

        services.AddScoped<IHubUserManager, HubUserManager>();

        return services;
    }

    /// <summary>
    /// Registers the HTTP client managers for the application
    /// </summary>
    internal static IServiceCollection RegisterHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClientInterceptor();

        // add a named HTTP client and handler
        //services.AddScoped(provider => provider.GetRequiredService<IHttpClientFactory>().CreateClient("HUB.Client").EnableIntercept(provider))
        //    .AddHttpClient("HUB.Client", client => client.BaseAddress = new Uri(configuration[ConfigurationKey.API.ServerUrl]!.TrimEnd('/')));

        services.AddHttpClient<HubUserManager>((provider, client) =>
        {
            client.EnableIntercept(provider);
            client.BaseAddress = new Uri(configuration[ConfigurationKey.API.ServerUrl]!.TrimEnd('/'));
        });

        return services;
    }

    /// <summary>
    /// Register authorization services
    /// </summary>
    internal static IServiceCollection RegisterClientAuthorization(this IServiceCollection services)
    {
        services.AddOptions();
        services.AddAuthorizationCore();

        services.AddScoped<HubAuthenticationStateProvider>();
        // use 'HubAuthenticationStateProvider' when system requests 'AuthenticationStateProvider'
        services.AddScoped<AuthenticationStateProvider, HubAuthenticationStateProvider>();

        return services;
    }
}

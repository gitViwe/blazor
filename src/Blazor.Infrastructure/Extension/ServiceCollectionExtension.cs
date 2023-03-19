using Blazor.Infrastructure.Configuration;
using Blazor.Infrastructure.Manager;
using Blazor.Infrastructure.MessageHandler;
using Blazor.Infrastructure.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Infrastructure.Extension;

internal static class ServiceCollectionExtension
{
    /// <summary>
    /// Registers the services required by the application
    /// </summary>
    internal static IServiceCollection RegisterClientServices(this IServiceCollection services)
    {
        services.AddSingleton<HubToggleVisibility>();
        services.AddSingleton<IStorageService, LocalStorageService>();

        return services;
    }

    /// <summary>
    /// Registers the HTTP client managers for the application
    /// </summary>
    internal static IServiceCollection RegisterHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuthenticationMessageHandler>();
        services.AddScoped<StatusCodeMessageHandler>();

        services.AddHttpClient<IHubUserManager, HubUserManager>((provider, client) =>
        {
            client.BaseAddress = new Uri(configuration[ConfigurationKey.API.ServerUrl]!.TrimEnd('/'));
        })
        /*.AddHttpMessageHandler<AuthenticationMessageHandler>()*/
        .AddHttpMessageHandler<StatusCodeMessageHandler>();

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

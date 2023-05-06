using Blazor.Infrastructure.Configuration;
using Blazor.Infrastructure.Manager;
using Blazor.Infrastructure.MessageHandler;
using Blazor.Infrastructure.Service;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

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
        services.AddTransient<IHttpInterceptorManager, HttpInterceptorManager>();

        return services;
    }

    /// <summary>
    /// Registers the HTTP client managers for the application
    /// </summary>
    internal static IServiceCollection RegisterHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuthenticatedRequestMessageHandler>();
        services.AddHttpClientInterceptor();

        services.AddHttpClient<AuthenticationTokenManager>((provider, client) =>
        {
            client.BaseAddress = new Uri(configuration[ConfigurationKey.API.ServerUrl]!.TrimEnd('/'));
        });

        services.AddHttpClient<IHubUserManager, HubUserManager>((provider, client) =>
        {
            client.EnableIntercept(provider);
            client.BaseAddress = new Uri(configuration[ConfigurationKey.API.ServerUrl]!.TrimEnd('/'));
        })
        .AddHttpMessageHandler<AuthenticatedRequestMessageHandler>();

        services.AddHttpClient<IFetchDataManager, FetchDataManager>((provider, client) =>
        {
            client.EnableIntercept(provider);
            client.BaseAddress = new Uri(configuration[ConfigurationKey.API.ServerUrl]!.TrimEnd('/'));
        })
        .AddHttpMessageHandler<AuthenticatedRequestMessageHandler>();

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
        services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<HubAuthenticationStateProvider>());

        return services;
    }

    internal static IServiceCollection RegisterFluentValidator(this IServiceCollection services)
    {
        return services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

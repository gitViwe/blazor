using Blazor.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace Blazor.Presentation;

public static class ConfigureServices
{
    public static IServiceCollection ConfigurePresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // add MudBlazor service https://mudblazor.com/getting-started/installation
        return services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
        })
        .ConfigureInfrastructureServices(configuration);
    }
}

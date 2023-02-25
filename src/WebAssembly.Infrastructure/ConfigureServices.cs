using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAssembly.Infrastructure.Extension;

namespace WebAssembly.Infrastructure;

/// <summary>
/// Implementation of the services registered in the DI container
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// Registers all required Infrastructure services
    /// </summary>
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .RegisterClientServices()
            .RegisterHttpClient(configuration)
            .RegisterClientAuthorization();
    }
}

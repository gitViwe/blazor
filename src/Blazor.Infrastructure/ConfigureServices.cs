using Blazor.Infrastructure.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.RegisterClientServices()
            .RegisterHttpClient(configuration)
            .RegisterClientAuthorization()
            .RegisterFluentValidator();
    }
}

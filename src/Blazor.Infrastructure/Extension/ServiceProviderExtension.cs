using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Infrastructure.Extension;

internal static class ServiceProviderExtension
{
    internal static async Task UseOpenTelemetryAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var storage = scope.ServiceProvider.GetRequiredService<IStorageService>();
        // clear local data
        await storage.RemoveAsync(StorageKey.OpenTelemetry.TraceContext);
        await storage.RemoveAsync(StorageKey.OpenTelemetry.SpanContext);
    }
}

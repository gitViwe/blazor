using Microsoft.Extensions.Configuration;

namespace Blazor.Shared.Extension;

public static class ConfigurationExtension
{
    public static string GetGatewayApiUri(this IConfiguration configuration) => configuration["BlazorConfiguration:Uri:GatewayApi"] ?? string.Empty;
    
    public static string GetSwaggerUri(this IConfiguration configuration)
        => GetGatewayApiUri(configuration) + configuration["BlazorConfiguration:Uri:SwaggerUiPath"];
    
    public static string GetGraphQlUri(this IConfiguration configuration)
        => GetGatewayApiUri(configuration) + configuration["BlazorConfiguration:Uri:GraphQlPath"];
    
    public static string GetHealthCheckUri(this IConfiguration configuration)
        => GetGatewayApiUri(configuration) + configuration["BlazorConfiguration:Uri:HealthCheckPath"];
    
    public static string GetSeqUiUri(this IConfiguration configuration) => configuration["BlazorConfiguration:Uri:SeqUiPath"] ?? string.Empty;
    
    public static string GetJaegerUiUri(this IConfiguration configuration) => configuration["BlazorConfiguration:Uri:JaegerUiPath"] ?? string.Empty;
    
    public static string GetGrafanaDashboardUri(this IConfiguration configuration) => configuration["BlazorConfiguration:Uri:GrafanaDashboardPath"] ?? string.Empty;
}
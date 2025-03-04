namespace Blazor.Shared.Extension;

public static class ConfigurationExtension
{
    private static string GetGatewayApiUri(this IConfiguration configuration)
        => configuration["BlazorConfiguration:Uri:GatewayApi"] ?? string.Empty;
    
    public static string GetSwaggerUri(this IConfiguration configuration)
        => GetGatewayApiUri(configuration) + "/swagger";
    
    public static string GetScalarUri(this IConfiguration configuration)
        => GetGatewayApiUri(configuration) + "/scalar";
    
    public static string GetGraphQlUri(this IConfiguration configuration)
        => GetGatewayApiUri(configuration) + "/graphql";
    
    public static string GetHealthCheckUri(this IConfiguration configuration)
        => GetGatewayApiUri(configuration) + "/healthchecks-ui";
    
    public static string GetSeqUiUri(this IConfiguration configuration)
        => configuration["BlazorConfiguration:Uri:SeqUi"] ?? string.Empty;
    
    public static string GetJaegerUiUri(this IConfiguration configuration)
        => configuration["BlazorConfiguration:Uri:JaegerUi"] ?? string.Empty;
    
    public static string GetGrafanaDashboardUri(this IConfiguration configuration)
        => configuration["BlazorConfiguration:Uri:GrafanaDashboard"] ?? string.Empty;
    
    public static string GetWebAuthnCredentialOptionsUri(this IConfiguration configuration, string displayName)
        => GetGatewayApiUri(configuration) + $"/auth/web-authentication/credential-options?displayName={displayName}";
    
    public static string GetWebAuthnCreateCredentialsUri(this IConfiguration configuration, string name)
        => GetGatewayApiUri(configuration) + $"/auth/web-authentication/credential/{Convert.ToBase64String(Encoding.UTF8.GetBytes(name))}";
    
    public static string GetWebAuthnAssertionOptionsUri(this IConfiguration configuration)
        => GetGatewayApiUri(configuration) + "/auth/web-authentication/assertion-options";
    
    public static string GetWebAuthnAssertionUri(this IConfiguration configuration)
        => GetGatewayApiUri(configuration) + "/auth/web-authentication/assertion";
    
    public static string GetFeatureFlagsUri(this IConfiguration configuration)
        => GetGatewayApiUri(configuration) + "/feature/feature-flags";
}
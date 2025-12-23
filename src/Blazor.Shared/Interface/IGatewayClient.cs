namespace Blazor.Shared.Interface;

public interface IGatewayClient
{
    public HttpClient HttpClient { get; }
}

internal class GatewayClient(HttpClient client) : IGatewayClient
{
    public const string ResilienceHandlerName = "GatewayClientResilienceHandler";
    public HttpClient HttpClient => client;
}
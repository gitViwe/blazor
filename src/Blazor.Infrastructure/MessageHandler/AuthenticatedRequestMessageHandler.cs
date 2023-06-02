using Blazor.Infrastructure.Manager;
using System.Net.Http.Headers;

namespace Blazor.Infrastructure.MessageHandler;

internal class AuthenticatedRequestMessageHandler : DelegatingHandler
{
    private readonly IStorageService _storageService;
    private readonly IOpenTelemetryService _openTelemetry;
    private readonly HubAuthenticationStateProvider _stateProvider;
    private readonly AuthenticationTokenManager _tokenManager;

    public AuthenticatedRequestMessageHandler(
        IStorageService storageService,
        IOpenTelemetryService openTelemetry,
        HubAuthenticationStateProvider stateProvider,
        AuthenticationTokenManager tokenManager)
    {
        _storageService = storageService;
        _openTelemetry = openTelemetry;
        _stateProvider = stateProvider;
        _tokenManager = tokenManager;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
    {
        await RefreshAuthenticationHeaderAsync(request, cancellationToken);
        await AddPropagationHeadersAsync(request);

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task RefreshAuthenticationHeaderAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // authentication scheme must be 'Bearer'
        if (request.Headers.Authorization?.Scheme != "Bearer")
        {
            // check the request URI of the intercepted request
            var absPath = request.RequestUri?.AbsolutePath;

            // request should not be an authenticated endpoint
            if (Shared.Route.AuthenticationAPI.IsAuthenticatedEndpoint(absPath!))
            {
                var claims = await _stateProvider.GetAuthenticationStateUserAsync();

                // only attempt refresh if claims are about to expire
                if (claims is not null && claims.HasExpiredClaims(2))
                {
                    await _tokenManager.RefreshTokenAsync();
                }

                // get the saved JWT token
                var savedToken = await _storageService.GetAsync<string>(StorageKey.Identity.AuthToken, cancellationToken);

                if (!string.IsNullOrWhiteSpace(savedToken))
                {
                    // use the saved token as the authorization header value
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
                }
            }
        }
    }

    private async Task AddPropagationHeadersAsync(HttpRequestMessage request)
    {
        var context = await _openTelemetry.GetContextResponseAsync();

        if (context?.TraceContext is not null)
        {
            if (!string.IsNullOrWhiteSpace(context.TraceContext.Traceparent))
            {
                request.Headers.Add(nameof(context.TraceContext.Traceparent), context.TraceContext.Traceparent);
            }

            if (!string.IsNullOrWhiteSpace(context.TraceContext.Tracestate))
            {
                request.Headers.Add(nameof(context.TraceContext.Tracestate), context.TraceContext.Tracestate);
            }
        }
    }
}

using System.Net.Http.Headers;

namespace Blazor.Infrastructure.MessageHandler;

internal class AuthenticationMessageHandler : DelegatingHandler
{
    private readonly IHubUserManager _userManager;
    private readonly IStorageService _storageService;
    private readonly HubAuthenticationStateProvider _stateProvider;
    private readonly ISnackbar _snackbar;

    public AuthenticationMessageHandler(
        IHubUserManager userManager,
        IStorageService storageService,
        HubAuthenticationStateProvider stateProvider,
        ISnackbar snackbar)
    {
        _userManager = userManager;
        _storageService = storageService;
        _stateProvider = stateProvider;
        _snackbar = snackbar;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
    {
        await RefreshAuthenticationHeaderAsync(request, cancellationToken);

        // process the request
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
            if (!string.IsNullOrWhiteSpace(absPath) && Shared.Route.AuthenticationAPI.IsAuthenticatedEndpoint(absPath!))
            {
                var claims = await _stateProvider.GetAuthenticationStateUserAsync();

                // only attempt refresh if claims are about to expire
                if (claims is not null && claims.HasExpiredClaims(2))
                {
                    var result = await _userManager.RefreshTokenAsync();

                    if (result.Succeeded())
                    {
                        // notify
                        _snackbar.Add(result.Message, Severity.Info);
                    }
                }

                // get the saved JWT token
                var savedToken = await _storageService.GetAsync<string>(StorageKey.Local.AuthToken, cancellationToken);

                if (!string.IsNullOrWhiteSpace(savedToken))
                {
                    // use the saved token as the authorization header value
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
                }
            }
        }
    }
}

using Blazor.Infrastructure.Manager;
using gitViwe.Shared.Abstraction;
using Microsoft.Extensions.Logging;
using Shared.Contract.ProblemDetail;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Blazor.Infrastructure.MessageHandler;

internal class AuthenticatedRequestMessageHandler : DelegatingHandler
{
    private readonly IStorageService _storageService;
    private readonly HubAuthenticationStateProvider _stateProvider;
    private readonly AuthenticationTokenManager _tokenManager;
    private readonly ISnackbar _snackbar;
    private readonly ILogger<AuthenticatedRequestMessageHandler> _logger;

    public AuthenticatedRequestMessageHandler(
        IStorageService storageService,
        HubAuthenticationStateProvider stateProvider,
        ISnackbar snackbar,
        ILogger<AuthenticatedRequestMessageHandler> logger,
        AuthenticationTokenManager tokenManager)
    {
        _storageService = storageService;
        _stateProvider = stateProvider;
        _snackbar = snackbar;
        _logger = logger;
        _tokenManager = tokenManager;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
    {
        await RefreshAuthenticationHeaderAsync(request, cancellationToken);

        // process the request
        var responseMessage = await base.SendAsync(request, cancellationToken);

        if (!responseMessage.IsSuccessStatusCode)
        {
            switch (responseMessage.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    await ClearAuthorizationTokensAsync(cancellationToken);
                    await ReadFromJsonAndNotifyAsync<DefaultProblemDetails>(responseMessage);
                    break;
                case HttpStatusCode.BadRequest:
                    await ReadFromJsonAndNotifyAsync<ValidationProblemDetails>(responseMessage);
                    break;
                default:
                    _snackbar.Add("An error occurred while making a request to the API.", Severity.Warning);
                    _logger.LogError("An error occurred while making a request to the API.\n{response}", await responseMessage.Content.ReadAsStringAsync(cancellationToken));
                    break;
            }
        }

        return responseMessage;
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
                    var result = await _tokenManager.RefreshTokenAsync();

                    if (result.Succeeded)
                    {
                        // notify
                        _snackbar.Add(result.Message, Severity.Info);
                    }
                    else
                    {
                        // notify
                        _snackbar.Add(result.Message, Severity.Warning);
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

    private async Task ReadFromJsonAndNotifyAsync<TProblem>(HttpResponseMessage responseMessage) where TProblem : IDefaultProblemDetails
    {
        var problem = await responseMessage.Content.ReadFromJsonAsync<TProblem>();

        _snackbar.Add(problem!.Detail, Severity.Warning);

        var stringBuilder = new StringBuilder()
            .AppendLine()
            .AppendLine($"Type    : {problem.Type}")
            .AppendLine($"Title   : {problem.Title}")
            .AppendLine($"Status  : {problem.Status}")
            .AppendLine($"Detail  : {problem.Detail}")
            .AppendLine($"Instance: {problem.Instance}");

        if (problem is IValidationProblemDetails validation)
        {
            stringBuilder.AppendLine($"Errors: {validation.ErrorsToDebugString()}");
        }

        // log errors
        _logger.LogError("An error occurred while making a request to the API.{response}", stringBuilder.ToString());
    }

    private async Task ClearAuthorizationTokensAsync(CancellationToken cancellationToken)
    {
        await _storageService.RemoveAsync(StorageKey.Local.AuthToken, cancellationToken);
        await _storageService.RemoveAsync(StorageKey.Local.AuthRefreshToken, cancellationToken);
        await _stateProvider.StateChangedAsync();
    }
}

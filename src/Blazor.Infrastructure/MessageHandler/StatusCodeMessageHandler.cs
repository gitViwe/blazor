using gitViwe.Shared.Abstraction;
using Microsoft.Extensions.Logging;
using Shared.Contract.ProblemDetail;
using System.Net;
using System.Net.Http.Json;

namespace Blazor.Infrastructure.MessageHandler;

internal class StatusCodeMessageHandler : DelegatingHandler
{
    private readonly IStorageService _storageService;
    private readonly HubAuthenticationStateProvider _stateProvider;
    private readonly ISnackbar _snackbar;
    private readonly ILogger<StatusCodeMessageHandler> _logger;

    public StatusCodeMessageHandler(
        IStorageService storageService,
        HubAuthenticationStateProvider stateProvider,
        ISnackbar snackbar,
        ILogger<StatusCodeMessageHandler> logger)
    {
        _storageService = storageService;
        _stateProvider = stateProvider;
        _snackbar = snackbar;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
    {
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
                    await ReadFromJsonAndNotifyAsync<DefaultProblemDetails>(responseMessage);
                    break;
            }
        }

        return responseMessage;
    }

    private async Task ReadFromJsonAndNotifyAsync<TProblem>(HttpResponseMessage responseMessage) where TProblem : IDefaultProblemDetails
    {
        var problem = await responseMessage.Content.ReadFromJsonAsync<TProblem>();
        string response = problem!.ToString()!;
        _snackbar.Add(problem!.Detail, Severity.Warning);

        // log errors
        _logger.LogError("An error occurred while making a request to the API.\n{response}", response);
    }

    private async Task ClearAuthorizationTokensAsync(CancellationToken cancellationToken)
    {
        await _storageService.RemoveAsync(StorageKey.Local.AuthToken, cancellationToken);
        await _storageService.RemoveAsync(StorageKey.Local.AuthRefreshToken, cancellationToken);
        _stateProvider.MarkUserAsLoggedOut();
    }
}

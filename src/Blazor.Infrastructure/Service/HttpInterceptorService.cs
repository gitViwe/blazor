using Microsoft.Extensions.Logging;
using Shared.Contract.ProblemDetail;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Toolbelt.Blazor;

namespace Blazor.Infrastructure.Service;

internal class HttpInterceptorService : IHttpInterceptorService
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly HubAuthenticationStateProvider _stateProvider;
    private readonly IStorageService _storageService;
    private readonly ISnackbar _snackbar;
    private readonly ILogger<HttpInterceptorService> _logger;

    public HttpInterceptorService(
        HttpClientInterceptor interceptor,
        HubAuthenticationStateProvider stateProvider,
        IStorageService storageService,
        ISnackbar snackbar,
        ILogger<HttpInterceptorService> logger)
    {
        _interceptor = interceptor;
        _stateProvider = stateProvider;
        _storageService = storageService;
        _snackbar = snackbar;
        _logger = logger;
    }
    public void DisposeEvent()
    {
        _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
        _interceptor.AfterSendAsync -= InterceptAfterHttpAsync;
    }

    public async Task InterceptAfterHttpAsync(object sender, HttpClientInterceptorEventArgs args)
    {
        try
        {
            var capturedContent = await args.GetCapturedContentAsync();

            if (capturedContent?.Headers?.ContentType?.MediaType == "application/problem+json")
            {
                var problem = await capturedContent.ReadFromJsonAsync<ValidationProblemDetails>();

                if (problem is not null)
                {
                    switch (problem.Status)
                    {
                        case (int)HttpStatusCode.Unauthorized:
                            await ClearAuthorizationTokensAsync();
                            LogErrorsAndNotify(problem);
                            break;
                        default:
                            LogErrors(problem);
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while making a request to the API.{message}", ex.Message);
        }
    }

    public Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs args)
    {
        return Task.CompletedTask;
    }

    public void RegisterEvent()
    {
        _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;
        _interceptor.AfterSendAsync += InterceptAfterHttpAsync;
    }

    private void LogErrorsAndNotify(IValidationProblemDetails problem)
    {
        var stringBuilder = new StringBuilder()
            .AppendLine()
            .AppendLine(problem.ToString())
            .AppendLine($"Errors  : {problem.ErrorsToDebugString()}");

        if (!string.IsNullOrWhiteSpace(problem?.Detail))
        {
            _snackbar.Add(problem?.Detail, Severity.Warning);
        }

        // log errors
        _logger.LogWarning("An error occurred while making a request to the API.{response}", stringBuilder.ToString());
    }

    private void LogErrors(IValidationProblemDetails problem)
    {
        var stringBuilder = new StringBuilder()
            .AppendLine()
            .AppendLine(problem.ToString())
            .AppendLine($"Errors  : {problem.ErrorsToDebugString()}");

        // log errors
        _logger.LogWarning("An error occurred while making a request to the API.{response}", stringBuilder.ToString());
    }

    private async Task ClearAuthorizationTokensAsync()
    {
        await _storageService.RemoveAsync(StorageKey.Identity.AuthToken);
        await _storageService.RemoveAsync(StorageKey.Identity.AuthRefreshToken);
        _stateProvider.MarkUserAsLoggedOut();
    }
}

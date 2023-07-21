using Microsoft.Extensions.Logging;
using Shared.Contract.ProblemDetail;
using System.Net;
using System.Net.Http.Json;
using Toolbelt.Blazor;

namespace Blazor.Infrastructure.Service;

internal class HttpInterceptorService : IHttpInterceptorService
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly HubAuthenticationStateProvider _stateProvider;
    private readonly IStorageService _storageService;
    private readonly ILogger<HttpInterceptorService> _logger;
    public Func<Task>? InterceptBeforeHttpTask { get; private set; }
    public Func<IValidationProblemDetails, Task>? InterceptAfterHttpTask { get; private set; }

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
        _logger = logger;
    }

    public void RegisterEvent(Func<Task>? interceptBeforeHttpTask = null, Func<IValidationProblemDetails, Task>? interceptAfterHttpTask = null)
    {
        InterceptBeforeHttpTask = interceptBeforeHttpTask;
        InterceptAfterHttpTask = interceptAfterHttpTask;

        _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;
        _interceptor.AfterSendAsync += InterceptAfterHttpAsync;
    }

    public void DisposeEvent()
    {
        _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
        _interceptor.AfterSendAsync -= InterceptAfterHttpAsync;
    }

    private async Task InterceptAfterHttpAsync(object sender, HttpClientInterceptorEventArgs args)
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
                            LogErrors(problem);
                            break;
                        default:
                            LogErrors(problem);
                            break;
                    }

                    if (InterceptAfterHttpTask is not null)
                    {
                        await InterceptAfterHttpTask(problem);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while making a request to the API.{message}", ex.Message);
        }
    }

    private Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs args)
    {
        return InterceptBeforeHttpTask is not null ? InterceptBeforeHttpTask() : Task.CompletedTask;
    }

    private void LogErrors(IValidationProblemDetails problem)
    {
        // log errors
        _logger.LogWarning("An error occurred while making a request to the API.{response}", problem?.ToString());
    }

    private async Task ClearAuthorizationTokensAsync()
    {
        await _storageService.RemoveAsync(StorageKey.Identity.AuthToken);
        await _storageService.RemoveAsync(StorageKey.Identity.AuthRefreshToken);
        _stateProvider.MarkUserAsLoggedOut();
    }
}

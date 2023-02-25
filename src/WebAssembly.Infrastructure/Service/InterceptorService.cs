using gitViwe.ProblemDetail.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Toolbelt.Blazor;
using WebAssembly.Infrastructure.Authentication;

namespace WebAssembly.Infrastructure.Service;

internal class InterceptorService : IInterceptorService
{
    private readonly IHubUserManager _userManager;
    private readonly HttpClientInterceptor _interceptor;
    private readonly IStorageService _storageService;
    private readonly HubAuthenticationStateProvider _stateProvider;
    private readonly ILogger<InterceptorService> _logger;
    private readonly ISnackbar _snackbar;

    public InterceptorService(
        IHubUserManager userManager,
        HttpClientInterceptor interceptor,
        IStorageService storageService,
        HubAuthenticationStateProvider stateProvider,
        ILogger<InterceptorService> logger,
        ISnackbar snackbar)
    {
        _userManager = userManager;
        _interceptor = interceptor;
        _storageService = storageService;
        _stateProvider = stateProvider;
        _logger = logger;
        _snackbar = snackbar;
    }

    public void Dispose()
    {
        _interceptor.BeforeSendAsync -= InterceptBeforeAsync;
        _interceptor.AfterSendAsync -= InterceptAfterAsync;
    }

    public void RegisterEvent()
    {
        _interceptor.BeforeSendAsync += InterceptBeforeAsync;
        _interceptor.AfterSendAsync += InterceptAfterAsync;
    }

    /// <summary>
    /// Evaluates the HTTP request before it is consumed by the API
    /// </summary>
    private async Task InterceptBeforeAsync(object sender, HttpClientInterceptorEventArgs e)
    {
        // check the request URI of the intercepted request
        var absPath = e.Request?.RequestUri?.AbsolutePath;

        // request should not be the one we use for the refresh token or login/logout action
        if (string.IsNullOrWhiteSpace(absPath) == false
            && Shared.Route.AuthenticationAPI.AcccountEndpoint.IsAnonymousEndpoint(absPath!) == false)
        {
            var claims = await _stateProvider.GetAuthenticationStateUserAsync();

            // only attempt refresh if claims are about to expire
            if (claims is not null && claims.HasExpiredClaims(2))
            {
                var result = await _userManager.RefreshTokenAsync();

                if (result.Succeeded)
                {
                    // notify
                    _snackbar.Add("Token Refreshed", Severity.Info);
                }
            }

            var token = await _storageService.GetAsync<string>(StorageKey.Local.AuthToken);
            // use the new token as the authorization header value
            e.Request!.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    /// <summary>
    /// Evaluates the HTTP response before it is consumed by the client
    /// </summary>
    private async Task InterceptAfterAsync(object sender, HttpClientInterceptorEventArgs e)
    {
        if (e.Response.IsSuccessStatusCode == false)
        {
            // Don't reference "e.Response.Content" directly to read the content
            var capturedContent = await e.GetCapturedContentAsync();
            var statusCode = e.Response.StatusCode;
            string response;

            switch (statusCode)
            {
                case HttpStatusCode.BadRequest:
                    // notify
                    var validationProblem = await capturedContent.ReadFromJsonAsync<ValidationProblemDetails>();
                    response = validationProblem!.ToString()!;
                    _snackbar.Add(validationProblem!.Detail, Severity.Warning);
                    break;
                default:
                    // notify
                    var problem = await capturedContent.ReadFromJsonAsync<DefaultProblemDetails>();
                    response = problem!.ToString()!;
                    _snackbar.Add(problem!.Detail, Severity.Warning);
                    break;
            }

            // log errors
            _logger.LogError(e.Exception, "An error occured while making a request to the API.\n{response}", response);

            e.Response.Content = null;
        }
    }
}

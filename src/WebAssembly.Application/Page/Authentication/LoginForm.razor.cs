using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Contract.Identity;
using Shared.Manager;
using WebAssembly.Infrastructure.Configuration;

namespace WebAssembly.Application.Page.Authentication;

public partial class LoginForm
{
    private readonly NavigationManager _navigationManager;
    private readonly IHubUserManager _userManager;
    private readonly ISnackbar _snackbar;

    public LoginForm(NavigationManager navigationManager, IHubUserManager userManager, ISnackbar snackbar)
    {
        _navigationManager = navigationManager;
        _userManager = userManager;
        _snackbar = snackbar;
    }

    /// <summary>
    /// The login view model
    /// </summary>
    public LoginRequest LoginRequest { get; set; } = new() { Email = "example@email.com", Password = "Password" };
    private static HubToggleVisibility ToggleVisibility => new();
    private bool IsProcessing { get; set; }
    private string RegisterRoute => Shared.Route.BlazorClient.Pages.Authentication.Register;

    /// <summary>
    /// Processes a login attempt when form validation passes
    /// </summary>
    /// <returns></returns>
    private async Task SubmitAsync()
    {
        IsProcessing = true;

        var response = await _userManager.LoginAsync(LoginRequest);

        // display response messages
        foreach (var message in response.Messages)
        {
            _snackbar.Add(message, response.Succeeded ? Severity.Success : Severity.Error);
        }

        if (response.Succeeded)
        {
            // redirect user to the home page
            _navigationManager.NavigateTo("/");
        }

        IsProcessing = false;
    }
}

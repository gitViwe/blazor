using Shared.Contract.Identity;
using System.Web;

namespace Blazor.Presentation.Page.Authentication;

public partial class LoginForm
{
    /// <summary>
    /// The login view model
    /// </summary>
    public LoginRequest LoginRequest { get; set; } = new() { Email = "example@email.com", Password = "Password" };
    private bool IsProcessing { get; set; }
    private static string RegisterRoute => BlazorClient.Pages.Authentication.Register;

    /// <summary>
    /// Processes a login attempt when form validation passes
    /// </summary>
    private async Task SubmitAsync()
    {
        IsProcessing = true;

        var response = await UserManager.LoginAsync(LoginRequest);

        // display response messages
        Snackbar.Add(response.Message, response.Succeeded ? Severity.Success : Severity.Error);

        if (response.Succeeded)
        {
            var url = new Uri(NavigationManager.Uri);
            var query = HttpUtility.ParseQueryString(url.Query);

            var parameters = new Dictionary<string, string>();
            foreach (string key in query)
            {
                parameters[key] = query[key]!;
            }

            if (parameters.TryGetValue("returnUrl", out var path) && !string.IsNullOrWhiteSpace(path))
            {
                NavigationManager.NavigateTo(path);
            }
            else
            {
                // redirect user to the home page
                NavigationManager.NavigateTo("/");
            }
        }

        IsProcessing = false;
    }
}

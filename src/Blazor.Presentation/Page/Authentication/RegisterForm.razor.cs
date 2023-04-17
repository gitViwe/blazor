using Shared.Contract.Identity;

namespace Blazor.Presentation.Page.Authentication;

public partial class RegisterForm
{
    /// <summary>
    /// The register view model
    /// </summary>
    public RegisterRequest RegisterRequest { get; set; } = new() { Email = "example@email.com", Password = "Password", PasswordConfirmation = "Password" };
    private bool IsProcessing { get; set; }
    private static string LoginRoute => BlazorClient.Pages.Authentication.Login;

    /// <summary>
    /// Processes a registration attempt when form validation passes
    /// </summary>
    /// <returns></returns>
    private async Task SubmitAsync()
    {
        IsProcessing = true;

        var response = await UserManager.RegisterAsync(RegisterRequest);

        // display response messages
        Snackbar.Add(response.Message, response.Succeeded ? Severity.Success : Severity.Error);

        if (response.Succeeded)
        {
            // redirect user to the home page
            NavigationManager.NavigateTo("/");
        }

        IsProcessing = false;
    }
}

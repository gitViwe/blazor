namespace Blazor.Component.Authentication.Redirect;

public sealed class RedirectToLogin : ComponentBase
{
    [Inject]
    public required NavigationManager NavigationManager { get; init; }
    
    protected override void OnInitialized()
    {
        NavigationManager.NavigateTo($"authentication/login?returnUrl={Uri.EscapeDataString(new Uri(NavigationManager.Uri).PathAndQuery)}");
    }
}
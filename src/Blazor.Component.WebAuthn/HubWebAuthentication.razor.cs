namespace Blazor.Component.WebAuthn;

public partial class HubWebAuthentication
    : ComponentBase, IComponentCancellationTokenSource
{
    public CancellationTokenSource Cts { get; } = new();
    
    [Inject]
    public required ISnackbar Snackbar { get; init; }
    
    [Inject]
    public required IHubWebAuthenticationManager HubWebAuthenticationManager { get; init; }

    private async Task RegisterAsync()
    {
        var response = await HubWebAuthenticationManager.RegisterAsync("viwe", Cts.Token);
        
        Snackbar.Add(response.Message, response.Succeeded ? Severity.Success : Severity.Error);
    }

    private async Task LoginAsync()
    {
        var response = await HubWebAuthenticationManager.LoginAsync(Cts.Token);
        
        Snackbar.Add(response.Message, response.Succeeded ? Severity.Success : Severity.Error);
    }

    public void Dispose()
    {
        Cts.Cancel();
        Cts.Dispose();
        GC.SuppressFinalize(this);
    }
}
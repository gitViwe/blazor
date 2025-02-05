namespace Blazor.Component.WebAuthn;

public partial class HubWebAuthentication
    : ComponentBase, IComponentCancellationTokenSource, IAsyncDisposable
{
    public CancellationTokenSource Cts { get; } = new();
    
    [Inject]
    public required ISnackbar Snackbar { get; init; }

    [Inject]
    public required IJSRuntime Runtime { get; init; }
    
    [Inject]
    public required HttpClient HttpClient { get; init; }
    
    [Inject]
    public required IConfiguration Configuration { get; init; }
    
    [Inject]
    public required ILogger<HubWebAuthentication> Logger { get; init; }

    private IJSObjectReference? _jsObjectReference;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsObjectReference = await Runtime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazor.Component.WebAuthn/js/hub-web-authentication.js");
            ArgumentNullException.ThrowIfNull(_jsObjectReference);
        }
    }

    private async Task RegisterAsync()
    {
        var response = await HubWebAuthenticationService.RegisterAsync(
            "viwe",
            Logger,
            HttpClient,
            Configuration,
            _jsObjectReference!,
            Cts.Token);
        
        Snackbar.Add(response.Message, response.Succeeded ? Severity.Success : Severity.Error);
    }

    private async Task LoginAsync()
    {
        var response = await HubWebAuthenticationService.LoginAsync(Logger,
            HttpClient,
            Configuration,
            _jsObjectReference!,
            Cts.Token);
        
        Snackbar.Add(response.Message, response.Succeeded ? Severity.Success : Severity.Error);
    }

    public async ValueTask DisposeAsync()
    {
        await Cts.CancelAsync();
        Cts.Dispose();
        if (_jsObjectReference is not null) await _jsObjectReference.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
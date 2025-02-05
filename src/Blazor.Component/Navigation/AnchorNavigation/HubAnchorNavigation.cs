namespace Blazor.Component.Navigation.AnchorNavigation;

public sealed class HubAnchorNavigation
    : ComponentBase, IComponentCancellationTokenSource, IAsyncDisposable
{
    private IJSObjectReference? _jsObjectReference;
    public CancellationTokenSource Cts { get; } = new();
    
    [Inject]
    public required IJSRuntime Runtime { get; init; }
    
    [Inject]
    public required NavigationManager Navigation { get; init; }
    
    protected override void OnInitialized() => Navigation.LocationChanged += OnLocationChanged;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsObjectReference = await Runtime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazor.Component/js/hub-web-authentication.js");
            ArgumentNullException.ThrowIfNull(_jsObjectReference);
        }
        await ScrollToFragment();
    }
    
    private async void OnLocationChanged(object? sender, LocationChangedEventArgs e) => await ScrollToFragment();
    
    private async Task ScrollToFragment()
    {
        var uri = new Uri(Navigation.Uri, UriKind.Absolute);
        
        if (uri.Fragment.StartsWith('#'))
        {
            // Handle text fragment (https://example.org/#test:~:text=Example)
            // https://github.com/WICG/scroll-to-text-fragment/
            var elementId = uri.Fragment[1..];
            var index = elementId.IndexOf(":~:", StringComparison.Ordinal);
            if (index > 0)
            {
                elementId = elementId.Substring(0, index);
            }

            if (false == string.IsNullOrEmpty(elementId))
            {
                await _jsObjectReference!.InvokeVoidAsync("scrollToElement", elementId);
            }
        }
    }
    
    public async ValueTask DisposeAsync()
    {
        Navigation.LocationChanged -= OnLocationChanged;
        await Cts.CancelAsync();
        Cts.Dispose();
        if (_jsObjectReference is not null) await _jsObjectReference.DisposeAsync();
    }
}
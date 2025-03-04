namespace Blazor.Component.Navigation.AnchorNavigation;

public sealed class HubAnchorNavigation
    : ComponentBase, IComponentCancellationTokenSource
{
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
            await ScrollToFragmentAsync();
        }
    }
    
    private void OnLocationChanged(object? sender, LocationChangedEventArgs e) => _ = ScrollToFragmentAsync();
    
    private async Task ScrollToFragmentAsync()
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
                await Runtime.InvokeVoidAsync("HubComponent.ScrollToElement", elementId);
            }
        }
    }
    
    public void Dispose()
    {
        Navigation.LocationChanged -= OnLocationChanged;
        Cts.Cancel();
        Cts.Dispose();
    }
}
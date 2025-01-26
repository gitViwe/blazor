namespace Blazor.Component.Navigation.AnchorNavigation;

public sealed class HubAnchorNavigation: ComponentBase, IDisposable
{
    [Inject]
    public required IJSRuntime Runtime { get; init; }
    
    [Inject]
    public required NavigationManager Navigation { get; init; }
    
    protected override void OnInitialized() => Navigation.LocationChanged += OnLocationChanged;

    protected override async Task OnAfterRenderAsync(bool firstRender) => await ScrollToFragment();
    
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
                await Runtime.InvokeVoidAsync("scrollToElement", elementId);
            }
        }
    }

    public void Dispose() => Navigation.LocationChanged -= OnLocationChanged;
}
namespace Blazor;

public partial class App : ComponentBase
{
    private readonly List<Assembly> _lazyLoadedAssemblies = [];
    
    [Inject]
    public required LazyAssemblyLoader AssemblyLoader { get; init; }
    
    [Inject]
    public required IJSRuntime Runtime { get; init; }
    
    private async Task OnNavigateAsync(NavigationContext context)
    {
        if (context.Path.StartsWith("test", StringComparison.InvariantCultureIgnoreCase))
        {
            var assemblies = await AssemblyLoader.LoadAssembliesAsync(HubLazyAssembly.WebAuthentication.Assemblies);
            _lazyLoadedAssemblies.AddRange(assemblies);
        }
    }
}
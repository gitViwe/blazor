namespace Blazor;

public partial class App : ComponentBase
{
    private readonly List<Assembly> _lazyLoadedAssemblies = [];
    
    [Inject]
    public required LazyAssemblyLoader AssemblyLoader { get; init; }
    
    private async Task OnNavigateAsync(NavigationContext context)
    {
        if (context.Path.StartsWith("test", StringComparison.InvariantCultureIgnoreCase))
        {
            var assemblies = await AssemblyLoader.LoadAssembliesAsync(["Blazor.Component.WebAuthn.dll"]);
            _lazyLoadedAssemblies.AddRange(assemblies);
        }
    }
}
namespace Blazor.Component.Page.Home;

public partial class HeroSection : ComponentBase
{
    [Inject]
    public required IJSRuntime Runtime { get; init; }
}
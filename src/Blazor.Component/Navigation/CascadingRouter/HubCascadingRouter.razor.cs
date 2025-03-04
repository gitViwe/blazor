namespace Blazor.Component.Navigation.CascadingRouter;

public partial class HubCascadingRouter : ComponentBase
{
    [Parameter, EditorRequired]
    public required RenderFragment ChildContent { get; init; }
}
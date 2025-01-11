namespace Blazor.Component.Card.ImageHover;

public partial class HubImageHover : ComponentBase
{
    [Parameter, EditorRequired]
    public required RenderFragment ChildContent { get; init; }
}
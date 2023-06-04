namespace Blazor.Presentation.Component;

public partial class HubCardHover
{
    [Parameter, EditorRequired] public required string Title { get; set; }
    [Parameter, EditorRequired] public required string Description { get; set; }
    [Parameter] public string ButtonText { get; set; } = string.Empty;
    [Parameter] public string ButtonLink { get; set; } = string.Empty;
    [Parameter] public string BackgroundImageUrl { get; set; } = string.Empty;
}

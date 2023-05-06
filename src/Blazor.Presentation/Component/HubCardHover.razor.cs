namespace Blazor.Presentation.Component;

public partial class HubCardHover
{
    [Parameter] public string Title { get; set; } = "Lorem ipsum";
    [Parameter] public string Description { get; set; } = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
    [Parameter] public string ButtonText { get; set; } = string.Empty;
    [Parameter] public string ButtonLink { get; set; } = string.Empty;
    [Parameter] public string BackgroundImageUrl { get; set; } = string.Empty;
}

namespace Blazor.Component.Typography.TextFadeIn;

public partial class HubTextFadeIn : ComponentBase
{
    [Parameter, EditorRequired]
    public required string TitleText { get; set; }
    
    [Parameter, EditorRequired]
    public required string RoleText { get; set; }
}
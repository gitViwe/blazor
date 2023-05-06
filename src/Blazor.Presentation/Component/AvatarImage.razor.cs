namespace Blazor.Presentation.Component;

public partial class AvatarImage
{
    [Parameter] public string Style { get; set; } = string.Empty;
    [Parameter, EditorRequired] public string Avatar { get; set; } = string.Empty;
    [Parameter, EditorRequired] public string UserName { get; set; } = string.Empty;
}

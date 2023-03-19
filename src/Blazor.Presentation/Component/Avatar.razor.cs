using Microsoft.AspNetCore.Components;

namespace Blazor.Presentation.Component;

public partial class Avatar
{
    [Parameter] public string Class { get; set; } = string.Empty;
    [Parameter] public string Style { get; set; } = string.Empty;
    [Parameter] public string AvatarImage { get; set; } = string.Empty;
    [Parameter] public string UserName { get; set; } = string.Empty;
}

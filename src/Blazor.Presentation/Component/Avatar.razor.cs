﻿namespace Blazor.Presentation.Component;

public partial class Avatar
{
    [Parameter] public string Class { get; set; } = string.Empty;
    [Parameter] public string Style { get; set; } = string.Empty;
    [Parameter, EditorRequired] public string AvatarSrc { get; set; } = string.Empty;
    [Parameter, EditorRequired] public string UserName { get; set; } = string.Empty;
}

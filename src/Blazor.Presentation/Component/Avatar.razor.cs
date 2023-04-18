namespace Blazor.Presentation.Component;

public partial class Avatar
{
    [Parameter] public string Class { get; set; } = string.Empty;
    [Parameter] public string Style { get; set; } = string.Empty;
    [Parameter] public string AvatarImage { get; set; } = string.Empty;
    [Parameter] public string UserName { get; set; } = "Demo User";

    protected override async Task OnInitializedAsync()
    {
        string avatar = await StorageService.GetAsync<string>(StorageKey.Local.AvatarImage);
        AvatarImage = string.IsNullOrWhiteSpace(avatar) ? string.Empty : avatar;
    }
}

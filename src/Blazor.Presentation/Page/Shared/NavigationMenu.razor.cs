namespace Blazor.Presentation.Page.Shared;

public partial class NavigationMenu
{
    public string AvatarSrc { get; set; } = string.Empty;
    public string UserName { get; set; } = "Demo User";

    protected override async Task OnInitializedAsync()
    {
        var user = await UserManager.CurrentUserAsync();

        AvatarSrc = user.FindFirst(HubClaimTypes.Avatar)?.Value ?? string.Empty;
        UserName = user.FindFirst(HubClaimTypes.UserName)?.Value ?? "Demo User";
    }

    private static string GetSwaggerEndpoint(string host)
    {
        return host + "/swagger";
    }
}

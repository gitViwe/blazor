namespace Blazor.Presentation.Page.Shared;

public partial class DefaultLayout
{
    public string AvatarSrc { get; set; } = string.Empty;
    public string UserName { get; set; } = "Demo User";

    // set custom theme defaults
    private readonly ThemeManagerTheme _themeManager = new()
    {
        Theme = new DefaultTheme(),
        DrawerClipMode = DrawerClipMode.Always,
        FontFamily = "Montserrat",
        DefaultBorderRadius = 6,
        AppBarElevation = 1,
        DrawerElevation = 1
    };

    // specifies the state of the side bar
    bool _drawerOpen = true;

    // toggles the state of the side bar
    void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    protected override async Task OnInitializedAsync()
    {
        var user = await UserManager.CurrentUserAsync();

        AvatarSrc = user.FindFirst(HubClaimTypes.Avatar)?.Value ?? string.Empty;
        UserName = user.FindFirst(HubClaimTypes.UserName)?.Value ?? "Demo User";
    }
}

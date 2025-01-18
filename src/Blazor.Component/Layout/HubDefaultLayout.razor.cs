namespace Blazor.Component.Layout;

public partial class HubDefaultLayout : LayoutComponentBase
{
    private bool IsDrawerOpen { get; set; }
    private bool IsDarkMode { get; set; }
    private void ToggleDrawer() => IsDrawerOpen = !IsDrawerOpen;
    
    private MudThemeProvider? MudThemeProviderReference { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && MudThemeProviderReference is not null)
        {
            var isSystemDarkMode = await MudThemeProviderReference.GetSystemPreference();
            await DarkModeChangedAsync(isSystemDarkMode);
            StateHasChanged();
        }
    }

    private Task DarkModeChangedAsync(bool newValue)
    {
        IsDarkMode = newValue;
        StateHasChanged();
        return Task.CompletedTask;
    }
}
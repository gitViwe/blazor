namespace Blazor.Component.Layout;

public class HubDefaultTheme : MudTheme
{
    public static HubDefaultTheme Default => new HubDefaultTheme();
    public HubDefaultTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = Colors.DeepPurple.Darken1,
            Background = "#d4e1e7",
            DrawerBackground = "#d4e1e7",
            DrawerText = "rgba(0,0,0, 0.7)",
        };

        PaletteDark = new PaletteDark()
        {
            Primary = Colors.DeepPurple.Darken1,
        };

        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "6px",
            DrawerWidthLeft = "400px",
        };

        Typography = new MudBlazor.Typography()
        {
            Default = new Default()
            {
                FontFamily = ["Montserrat", "Helvetica", "Arial", "sans-serif"],
            },
        };
        Shadows = new Shadow();
        ZIndex = new ZIndex();
    }
}
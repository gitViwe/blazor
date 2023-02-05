namespace WebAssembly.Infrastructure.Configuration;

public class HubToggleVisibility
{
    private bool IsVisible = false;
    public InputType InputType { get; private set; } = InputType.Password;
    public string InputIcon { get; private set; } = Icons.Material.Filled.VisibilityOff;

    public void ToggleVisibility()
    {
        if (IsVisible)
        {
            IsVisible = false;
            InputType = InputType.Password;
            InputIcon = Icons.Material.Filled.VisibilityOff;
        }
        else
        {
            IsVisible = true;
            InputType = InputType.Text;
            InputIcon = Icons.Material.Filled.Visibility;
        }
    }
}

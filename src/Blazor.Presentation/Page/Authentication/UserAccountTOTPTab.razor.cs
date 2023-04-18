namespace Blazor.Presentation.Page.Authentication;

public partial class UserAccountTOTPTab
{
    [Parameter] public string QrCodeImage { get; set; } = string.Empty;

    private async Task GetQrAsync()
    {
        var result = await UserManager.GetQrCodeAsync();
        if (result.Succeeded)
        {
            QrCodeImage = result.Data;
        }
        else
        {
            Snackbar.Add(result.Message, Severity.Warning);
        }
    }
}

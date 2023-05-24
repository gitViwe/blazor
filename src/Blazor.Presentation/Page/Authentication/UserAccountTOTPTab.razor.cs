using Blazor.Presentation.Constant;
using Shared.Contract.Identity;

namespace Blazor.Presentation.Page.Authentication;

public partial class UserAccountTOTPTab
{
    [Parameter] public string QrCodeImage { get; set; } = Asset.Image.TwoFactorAuthentication;
    private TOTPVerifyRequest TOTPVerifyModel { get; set; } = new();
    private bool IsProcessing { get; set; }

    private async Task GetQrAsync()
    {
        IsProcessing = true;

        var result = await UserManager.GetQrCodeAsync();
        if (result.Succeeded)
        {
            QrCodeImage = result.Data;
        }

        Snackbar.Add(result.Message, result.Succeeded ? Severity.Success : Severity.Warning);

        IsProcessing = false;
    }

    private async Task SubmitAsync()
    {
        IsProcessing = true;

        var result = await UserManager.VerifyTOTPAsync(TOTPVerifyModel);
        if (result.Succeeded)
        {
            QrCodeImage = Asset.Image.TwoFactorAuthentication;
        }

        Snackbar.Add(result.Message, result.Succeeded ? Severity.Success : Severity.Warning);

        IsProcessing = false;
    }
}

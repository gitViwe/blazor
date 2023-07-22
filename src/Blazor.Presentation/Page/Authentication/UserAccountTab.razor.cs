using Microsoft.AspNetCore.Components.Forms;
using Shared.Contract.Identity;

namespace Blazor.Presentation.Page.Authentication;

public partial class UserAccountTab
{
    public string AvatarImage { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = "Demo User";
    public UpdateUserRequest UpdateUserModel { get; set; } = new();
    private bool IsProcessing { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var user = await UserManager.CurrentUserAsync();

        AvatarImage = user.FindFirst(HubClaimTypes.Avatar)?.Value ?? string.Empty;
        UserName = user.FindFirst(HubClaimTypes.UserName)?.Value ?? "Demo User";
        Email = user.FindFirst(HubClaimTypes.Email)?.Value ?? string.Empty;
        FirstName = user.FindFirst(HubClaimTypes.FirstName)?.Value ?? string.Empty;
        LastName = user.FindFirst(HubClaimTypes.LastName)?.Value ?? string.Empty;
        UpdateUserModel = new() { FirstName = FirstName, LastName = LastName };
    }

    private async Task UploadFileAsync(IBrowserFile file)
    {
        IsProcessing = true;

        try
        {
            var response = await UserManager.UploadImageAsync(file.OpenReadStream(), file.Name);

            if (response.Succeeded)
            {
                await GetUserDetailAsync();
            }

            Snackbar.Add(response.Message, response.Succeeded ? Severity.Success : Severity.Warning);
        }
        catch (IOException)
        {
            long maxAllowedSize = 500 * 1024;
            Snackbar.Add($"File size exceeds the maximum allowed limit of {Formatter.FormatSize(maxAllowedSize)}", Severity.Warning);
        }

        IsProcessing = false;
    }

    private async Task SubmitAsync()
    {
        IsProcessing = true;

        var response = await UserManager.UpdateDetailsAsync(UpdateUserModel);

        if (response.Succeeded)
        {
            await GetUserDetailAsync();
        }

        Snackbar.Add(response.Message, response.Succeeded ? Severity.Success : Severity.Warning);

        IsProcessing = false;
    }

    private async Task GetUserDetailAsync()
    {
        var userResponse = await UserManager.GetUserDetailAsync();

        if (userResponse?.Data is not null && userResponse.Succeeded)
        {
            AvatarImage = userResponse.Data.ProfileImage.Image.Url;
            UserName = userResponse.Data.Username;
            Email = userResponse.Data.Email;
            FirstName = userResponse.Data.FirstName;
            LastName = userResponse.Data.LastName;
        }
    }
}

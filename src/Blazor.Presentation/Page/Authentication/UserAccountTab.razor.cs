using Microsoft.AspNetCore.Components.Forms;

namespace Blazor.Presentation.Page.Authentication;

public partial class UserAccountTab
{
    [Parameter] public string AvatarImage { get; set; } = string.Empty;
    [Parameter] public string Email { get; set; } = string.Empty;
    [Parameter] public string FirstName { get; set; } = string.Empty;
    [Parameter] public string LastName { get; set; } = string.Empty;
    [Parameter] public string UserName { get; set; } = "Demo User";

    protected override async Task OnInitializedAsync()
    {
        string avatar = await StorageService.GetAsync<string>(StorageKey.Local.AvatarImage);
        AvatarImage = string.IsNullOrWhiteSpace(avatar) ? string.Empty : avatar;
        var authState = await AuthenticationState.GetAuthenticationStateAsync();
        if (authState?.User?.Identity is not null && authState.User.Identity.IsAuthenticated)
        {
            Email = authState.User.FindFirst("email")?.Value ?? string.Empty;
            UserName = authState.User.FindFirst("unique_name")?.Value ?? string.Empty;
        }
    }

    private async Task UploadFileAsync(IBrowserFile file)
    {
        try
        {
            using var stream = file.OpenReadStream();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            byte[] fileBytes = memoryStream.ToArray();

            // Create a folder
            string folderPath = AppDomain.CurrentDomain.BaseDirectory + Path.Combine("image", UserName.Replace(" ", string.Empty));
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, file.Name);
            File.WriteAllBytes(filePath, fileBytes);

            string fileBase64String = $"data:{file.ContentType};base64," + Convert.ToBase64String(fileBytes);
            await StorageService.SetAsync(StorageKey.Local.AvatarImage, fileBase64String);
            AvatarImage = fileBase64String;
        }
        catch (IOException)
        {
            long maxAllowedSize = 500 * 1024;
            Snackbar.Add($"File size exceeds the maximum allowed limit of {FileSizeFormatter.FormatSize(maxAllowedSize)}", Severity.Warning);
        }
        //TODO upload the files to the server
    }
}

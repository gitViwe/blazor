namespace Blazor.Infrastructure.Extension;

public static class SnackBarExtension
{
    public static Task AddWarningAsync(this ISnackbar snackbar, string? message)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            snackbar.Add(message, Severity.Warning);
        }
        return Task.CompletedTask;
    }
}

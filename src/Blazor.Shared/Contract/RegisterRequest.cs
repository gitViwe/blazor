namespace Blazor.Shared.Contract;

public class RegisterRequest
{
    [Required]
    public string UserName { get; init; }  = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;
    
    [Required]
    [MinLength(6)]
    public string Password { get; init; } = string.Empty;
    
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    public string PasswordConfirmation { get; init; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
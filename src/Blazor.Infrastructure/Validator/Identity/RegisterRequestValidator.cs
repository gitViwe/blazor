using FluentValidation;
using Shared.Contract.Identity;

namespace Blazor.Infrastructure.Validator.Identity;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.UserName)
        .NotEmpty().WithMessage("Username is required.")
        .MinimumLength(5).WithMessage("Username must be at least 5 characters long.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email Address is required.")
            .EmailAddress().WithMessage("Email Address is invalid.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

        RuleFor(x => x.PasswordConfirmation)
            .Equal(x => x.Password).WithMessage("Password and Confirm Password do not match.");
    }
}

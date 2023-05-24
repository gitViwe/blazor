using FluentValidation;
using Shared.Contract.Identity;

namespace Blazor.Infrastructure.Validator.Identity;

public class TOTPVerifyRequestValidator : AbstractValidator<TOTPVerifyRequest>
{
    public TOTPVerifyRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required.");
    }
}

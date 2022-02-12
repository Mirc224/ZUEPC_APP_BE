using FluentValidation;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Auth.Commands;
using MVCAPIDemo.Localization;

namespace MVCAPIDemo.Auth.Validators;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator(IStringLocalizer<DataAnnotations> localizer)
    {
        RuleFor(x => x.Token)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.RefreshToken)
            .NotNull()
            .NotEmpty();
    }
}

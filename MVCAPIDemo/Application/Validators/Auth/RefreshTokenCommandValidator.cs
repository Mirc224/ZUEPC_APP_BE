using FluentValidation;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Application.Commands.Auth;
using MVCAPIDemo.Localization;

namespace MVCAPIDemo.Application.Validators.Auth;

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

using FluentValidation;
using Microsoft.Extensions.Localization;
using ZUEPC.Application.Auth.Commands;
using ZUEPC.Localization;

namespace ZUEPC.Application.Auth.Validators;

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

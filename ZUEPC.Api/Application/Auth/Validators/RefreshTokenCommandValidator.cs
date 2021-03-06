using FluentValidation;
using Microsoft.Extensions.Localization;
using ZUEPC.Application.Auth.Commands.RefreshTokens;
using ZUEPC.Localization;

namespace ZUEPC.Application.Auth.Validators;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.RefreshToken)
            .NotNull()
            .NotEmpty();
    }
}

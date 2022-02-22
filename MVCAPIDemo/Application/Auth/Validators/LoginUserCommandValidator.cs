using FluentValidation;
using Microsoft.Extensions.Localization;
using ZUEPC.Application.Auth.Commands;
using ZUEPC.Localization;

namespace ZUEPC.Application.Auth.Validators;

public class LoginUserCommandValidator: AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator(IStringLocalizer<DataAnnotations> localizer)
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty();
    }
}

using FluentValidation;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Auth.Commands;
using MVCAPIDemo.Localization;

namespace MVCAPIDemo.Auth.Validators;

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

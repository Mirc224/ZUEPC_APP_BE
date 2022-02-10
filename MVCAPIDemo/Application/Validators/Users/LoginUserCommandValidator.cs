using FluentValidation;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Application.Commands.Users;
using MVCAPIDemo.Localization;

namespace MVCAPIDemo.Application.Validators.Users;

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

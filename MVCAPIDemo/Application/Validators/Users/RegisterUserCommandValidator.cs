using DataAccess.Data;
using FluentValidation;
using MVCAPIDemo.Application.Commands.Users;

namespace MVCAPIDemo.Application.Validators.Users;

public class RegisterUserCommandValidator: AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(IUserData repository)
    {
        RuleFor(x=> x.FirstName)
            .NotEmpty()
            .NotNull();
        RuleFor(x=> x.LastName)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Email)
            .EmailAddress();
    }
}

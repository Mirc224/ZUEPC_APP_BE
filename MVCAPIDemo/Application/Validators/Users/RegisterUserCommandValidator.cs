using DataAccess.Data;
using FluentValidation;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Application.Commands.Users;
using MVCAPIDemo.Localization;

namespace MVCAPIDemo.Application.Validators.Users;

public class RegisterUserCommandValidator: AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(IStringLocalizer<DataAnnotations> localizer, IUserData repository)
    {
        RuleFor(x=> x.FirstName)
            .NotEmpty()
            .NotNull();
        RuleFor(x=> x.LastName)
            .NotEmpty()
            .NotNull();
        RuleFor(x => x.Email)
            .EmailAddress()
            .Must(email => !AlreadyExists(repository, email))
            .WithMessage(localizer["EmailAlreadyUsed"]);
    }

    private bool AlreadyExists(IUserData repository, string email)
    {
        var result = repository.GetUserByEmail(email);
        result.Wait();
        return result.Result != null;
    }
}

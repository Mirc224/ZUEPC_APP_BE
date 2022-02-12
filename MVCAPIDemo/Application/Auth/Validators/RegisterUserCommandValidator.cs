using Dapper;
using DataAccess.Data.User;
using FluentValidation;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Auth.Commands;
using MVCAPIDemo.Localization;

namespace MVCAPIDemo.Auth.Validators;

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
        var result =  repository.GetUserByEmailAsync(email);
        result.Wait();
        return result.Result != null;
    }
}

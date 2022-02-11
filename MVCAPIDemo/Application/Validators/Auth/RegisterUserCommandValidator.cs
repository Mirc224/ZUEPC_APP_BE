using Dapper;
using DataAccess.Data.User;
using FluentValidation;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Application.Commands.Auth;
using MVCAPIDemo.Localization;

namespace MVCAPIDemo.Application.Validators.Auth;

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
		var builder = new SqlBuilder();
		builder.Select("*");
		builder.Where("Email = @Email");
        var result =  repository.GetUsersAsync(new { Email = email}, builder);
        result.Wait();
        return result.Result.Any();
    }
}

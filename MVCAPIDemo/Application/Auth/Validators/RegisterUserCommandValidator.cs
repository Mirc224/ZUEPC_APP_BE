using DataAccess.Data.User;
using FluentValidation;
using Microsoft.Extensions.Localization;
using ZUEPC.Application.Auth.Commands;
using ZUEPC.Localization;

namespace ZUEPC.Application.Auth.Validators;
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
            .WithMessage(localizer[DataAnnotationsKeyConstants.EMAIL_ALREADY_USED]);
    }

    private bool AlreadyExists(IUserData repository, string email)
    {
        var result =  repository.GetUserByEmailAsync(email);
        result.Wait();
        return result.Result != null;
    }
}

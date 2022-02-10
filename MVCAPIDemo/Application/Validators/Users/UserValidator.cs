using FluentValidation;
using MVCAPIDemo.Application.Domain;

namespace MVCAPIDemo.Application.Validators.Users;

public class UserValidator : AbstractValidator<User>
{
	public UserValidator()
	{
		RuleFor(x => x.Email)
			.NotNull();
		RuleForEach(x => x.Roles)
			.IsInEnum();
	}
}

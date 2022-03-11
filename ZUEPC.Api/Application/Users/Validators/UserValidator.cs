using FluentValidation;
using Users.Base.Domain;

namespace ZUEPC.Application.Users.Validators;

public class UserValidator : AbstractValidator<User>
{
	public UserValidator()
	{
		RuleFor(x => x.Email)
			.NotNull();
		//RuleForEach(x => x.Roles)
		//	.IsInEnum();
	}
}

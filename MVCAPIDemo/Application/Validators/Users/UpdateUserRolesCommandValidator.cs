using FluentValidation;
using MVCAPIDemo.Application.Commands.Users;

namespace MVCAPIDemo.Application.Validators.Users;

public class UpdateUserRolesCommandValidator : AbstractValidator<UpdateUserCommand>
{
	public UpdateUserRolesCommandValidator()
	{
		RuleFor(x => x.UserRoles)
			.NotNull();

		RuleForEach(x => x.UserRoles).IsInEnum();
	}
}

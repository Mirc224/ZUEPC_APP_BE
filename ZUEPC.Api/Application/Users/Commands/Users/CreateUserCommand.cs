using MediatR;

namespace ZUEPC.Api.Application.Users.Commands.Users;

public class CreateUserCommand : IRequest<CreateUserCommandResponse>
{
	public string? Email { get; set; }
	public string? Password { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
}

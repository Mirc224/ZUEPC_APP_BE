using MediatR;

namespace ZUEPC.Api.Application.Users.Commands.UserRoles;

public class CreateUserRoleCommand : IRequest<CreateUserRoleCommandResponse>
{
	public long UserId { get; set; }
	public long RoleId { get; set; }
}

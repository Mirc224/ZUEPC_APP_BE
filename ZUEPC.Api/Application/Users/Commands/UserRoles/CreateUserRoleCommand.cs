using MediatR;
using ZUEPC.Base.Enums.Users;

namespace ZUEPC.Api.Application.Users.Commands.UserRoles;

public class CreateUserRoleCommand : IRequest<CreateUserRoleCommandResponse>
{
	public long UserId { get; set; }
	public RoleType RoleType { get; set; }
}

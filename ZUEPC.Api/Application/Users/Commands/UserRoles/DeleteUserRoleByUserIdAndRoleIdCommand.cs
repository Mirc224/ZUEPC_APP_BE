using MediatR;
using ZUEPC.Base.Enums.Users;

namespace ZUEPC.Api.Application.Users.Commands.UserRoles;

public class DeleteUserRoleByUserIdAndRoleIdCommand : IRequest<DeleteUserRoleByUserIdAndRoleIdCommandResponse>
{
	public long UserId { get; set; }
	public RoleType RoleType { get; set; }
}

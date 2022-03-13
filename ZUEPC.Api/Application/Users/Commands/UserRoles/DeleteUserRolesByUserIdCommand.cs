using MediatR;

namespace ZUEPC.Api.Application.Users.Commands.UserRoles;

public class DeleteUserRolesByUserIdCommand
	: IRequest<DeleteUserRolesByUserIdCommandResponse>
{
	public long UserId { get; set; }
}

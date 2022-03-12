using MediatR;
using ZUEPC.Base.Enums.Users;

namespace ZUEPC.Api.Application.Users.Commands.UserRoles;

public class UpdateUserRolesCommand : IRequest<UpdateUserRolesCommandResponse>
{
	public long UserId { get; set; }
	public IEnumerable<RoleType>? RolesToInsert { get; set; }	
	public IEnumerable<RoleType>? RolesToDelete { get; set; }	
}

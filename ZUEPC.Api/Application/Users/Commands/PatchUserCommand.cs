using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Users.Base.Domain;
using ZUEPC.Base.Enums.Users;

namespace ZUEPC.Application.Users.Commands;

public class PatchUserCommand : IRequest<PatchUserCommandResponse>
{
	public int UserId { get; set; }
	public JsonPatchDocument<User>? AppliedPatch { get; set; }
	public HashSet<RoleType>? UserRoles { get; set; }
}

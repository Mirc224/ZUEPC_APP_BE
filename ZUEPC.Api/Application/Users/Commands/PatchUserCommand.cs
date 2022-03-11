using ZUEPC.DataAccess.Enums;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Users.Base.Domain;

namespace ZUEPC.Application.Users.Commands;

public class PatchUserCommand : IRequest<PatchUserCommandResponse>
{
	public int UserId { get; set; }
	public JsonPatchDocument<User>? AppliedPatch { get; set; }
	public HashSet<RoleType>? UserRoles { get; set; }
}

using DataAccess.Enums;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Users.Base.Domain;

namespace MVCAPIDemo.Users.Commands;

public class UpdateUserCommand : IRequest<UpdateUserCommandResponse>
{
	public int UserId { get; set; }
	public JsonPatchDocument<User>? AppliedPatch { get; set; }
	public HashSet<RolesType>? UserRoles { get; set; }
}

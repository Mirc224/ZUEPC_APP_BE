using DataAccess.Enums;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using MVCAPIDemo.Application.Domain;
using System.Text.Json.Serialization;

namespace MVCAPIDemo.Application.Commands.Users;

public class UpdateUserRolesCommand : IRequest<UpdateUserRolesCommandResponse>
{
	public int UserId { get; set; }
	public JsonPatchDocument<User> AppliedPatch { get; set; }
	public HashSet<RolesType> UserRoles { get; set; }
}

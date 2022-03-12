using MediatR;
using ZUEPC.Api.Application.Users.Queries.UserRoles;
using ZUEPC.Application.Users.Queries.Users;
using ZUEPC.Base.Enums.Users;
using ZUEPC.Common.Extensions;
using ZUEPC.Users.Base.Domain;

namespace ZUEPC.Api.Application.Users.Commands.UserRoles;

public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, UpdateUserRolesCommandResponse>
{
	private readonly IMediator _mediator;

	public UpdateUserRolesCommandHandler(IMediator mediator)
	{
		_mediator = mediator;
	}
	public async Task<UpdateUserRolesCommandResponse> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
	{
		GetUserQueryResponse userResponse = await _mediator.Send(new GetUserQuery() { Id = request.UserId});
		if(!userResponse.Success)
		{
			return new() { Success = false };
		}
		GetUserRolesByUserIdQueryResponse userRolesResponse = await _mediator.Send(new GetUserRolesByUserIdQuery() { UserId = request.UserId});
		if(!userRolesResponse.Success || userRolesResponse.Data is null)
		{
			return new() { Success = false };
		}

		IEnumerable<RoleType>? actualUserRoles = userRolesResponse.Data.Select(x => (RoleType)x.RoleId);

		await DeleteUserRoles(request.UserId, actualUserRoles, request.RolesToDelete);
		await InsertUserRoles(request.UserId, actualUserRoles, request.RolesToInsert);
		return new() { Success = true };
	}

	private async Task InsertUserRoles(
		long userId,
		IEnumerable<RoleType>? actualRoles,
		IEnumerable<RoleType>? rolesToInsert)
	{
		if(rolesToInsert is null || actualRoles is null)
		{
			return;
		}

		IEnumerable<RoleType> addedRoles = rolesToInsert.Except(actualRoles);
		
		foreach (RoleType role in addedRoles.OrEmptyIfNull())
		{
			await _mediator.Send(new CreateUserRoleCommand() { UserId = userId, RoleType = role });
		}
	}

	private async Task DeleteUserRoles(
		long userId, 
		IEnumerable<RoleType>? actualRoles,
		IEnumerable<RoleType>? rolesToDelete
		)
	{
		if (rolesToDelete is null || actualRoles is null)
		{
			return;
		}
		
		IEnumerable<RoleType> removedRoles = actualRoles.Intersect(rolesToDelete);
		foreach (RoleType role in removedRoles.OrEmptyIfNull())
		{
			await _mediator.Send(new DeleteUserRoleByUserIdAndRoleIdCommand() { UserId = userId, RoleType = role });
		}
	}
}

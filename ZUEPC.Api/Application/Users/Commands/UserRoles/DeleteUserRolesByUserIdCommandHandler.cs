using MediatR;
using ZUEPC.DataAccess.Data.Users;

namespace ZUEPC.Api.Application.Users.Commands.UserRoles;

public class DeleteUserRolesByUserIdCommandHandler : 
	IRequestHandler<DeleteUserRolesByUserIdCommand, DeleteUserRolesByUserIdCommandResponse>
{
	private readonly IUserRoleData _repository;

	public DeleteUserRolesByUserIdCommandHandler(IUserRoleData repository)
	{
		_repository = repository;
	}

	public async Task<DeleteUserRolesByUserIdCommandResponse> Handle(DeleteUserRolesByUserIdCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeleteUserRoleByUserIdAsync(request.UserId);
		return new() { Success = true };
	}
}

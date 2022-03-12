using MediatR;
using ZUEPC.DataAccess.Data.Users;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.Api.Application.Users.Commands.UserRoles;

public class DeleteUserRoleByUserIdAndRoleIdCommandHandler 
	: IRequestHandler<DeleteUserRoleByUserIdAndRoleIdCommand, DeleteUserRoleByUserIdAndRoleIdCommandResponse>
{
	private readonly IUserRoleData _repository;

	public DeleteUserRoleByUserIdAndRoleIdCommandHandler(IUserRoleData repository)
	{
		_repository = repository;
	}

	public async Task<DeleteUserRoleByUserIdAndRoleIdCommandResponse> Handle(DeleteUserRoleByUserIdAndRoleIdCommand request, CancellationToken cancellationToken)
	{
		UserRoleModel? currentModel = await _repository.GetUserRoleByUserIdAndRoleIdAsync(request.UserId, (long)request.RoleType);
		if (currentModel is null)
		{
			return new() { Success = false };
		}
		int rowsDeleted = await _repository.DeleteModelByIdAsync(currentModel.Id);
		return new() { Success = true };
	}
}

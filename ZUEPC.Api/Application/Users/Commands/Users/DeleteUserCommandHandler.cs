using DataAccess.Data.User;
using MediatR;
using ZUEPC.Api.Application.Users.Commands.UserRoles;
using ZUEPC.Application.Users.Queries.Users;

namespace ZUEPC.Api.Application.Users.Commands.Users;

public class DeleteUserCommandHandler :
	IRequestHandler<DeleteUserCommand, DeleteUserCommandResponse>
{
	private readonly IMediator _mediator;
	private readonly IUserData _repository;

	public DeleteUserCommandHandler(IMediator mediator, IUserData repository)
	{
		_mediator = mediator;
		_repository = repository;
	}
	public async Task<DeleteUserCommandResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
	{
		long userId = request.Id;
		GetUserQueryResponse model = await _mediator.Send(new GetUserQuery() { Id = userId});

		if (!model.Success)
		{
			return new() { Success = false };
		}
		await _mediator.Send(new DeleteUserRolesByUserIdCommand() { UserId = userId});
		await _repository.DeleteModelByIdAsync(userId);
		return new() { Success = true };
	}
}

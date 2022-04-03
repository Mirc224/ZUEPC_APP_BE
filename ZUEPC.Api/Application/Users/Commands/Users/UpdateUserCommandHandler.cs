using AutoMapper;
using DataAccess.Data.User;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.Api.Application.Users.Commands.Users;

public class UpdateUserCommandHandler :
	UpdateSimpleModelCommandHandlerBase<IUserData, UserModel, long>,
	IRequestHandler<UpdateUserCommand, UpdateUserCommandResponse>
{
	public UpdateUserCommandHandler(IMapper mapper, IUserData repository)
		: base(mapper, repository) { }
	public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
	{
		return await ProcessUpdateCommandFromRequestAsync
			<UpdateUserCommand,
			UpdateUserCommandResponse>(request);
	}
}

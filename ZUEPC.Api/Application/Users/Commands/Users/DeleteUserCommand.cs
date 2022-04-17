using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Api.Application.Users.Commands.Users;

public class DeleteUserCommand : 
	EPCDeleteCommandBase<long>,
	IRequest<DeleteUserCommandResponse>
{
}

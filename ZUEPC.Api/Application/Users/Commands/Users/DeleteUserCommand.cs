using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Api.Application.Users.Commands.Users;

public class DeleteUserCommand : 
	DeleteModelCommandBase<long>,
	IRequest<DeleteUserCommandResponse>
{
}

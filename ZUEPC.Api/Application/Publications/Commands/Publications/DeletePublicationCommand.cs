using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class DeletePublicationCommand : 
	EPCDeleteCommandBase<long>,
	IRequest<DeletePublicationCommandResponse>
{
}

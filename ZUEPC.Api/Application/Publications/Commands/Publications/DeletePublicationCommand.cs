using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class DeletePublicationCommand : 
	EPCDeleteModelCommandBase<long>,
	IRequest<DeletePublicationCommandResponse>
{
}

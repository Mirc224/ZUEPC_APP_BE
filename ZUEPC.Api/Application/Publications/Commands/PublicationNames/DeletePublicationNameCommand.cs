using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.PublicationNames;

public class DeletePublicationNameCommand : 
	EPCDeleteModelCommandBase<long>, 
	IRequest<DeletePublicationNameCommandResponse>
{
}

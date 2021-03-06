using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.PublicationIdentifiers;

public class DeletePublicationIdentifierCommand : 
	EPCDeleteCommandBase<long>, 
	IRequest<DeletePublicationIdentifierCommandResponse>
{
}

using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.RelatedPublications.Commands;

public class DeleteRelatedPublicationCommand : 
	EPCDeleteModelCommandBase<long>, 
	IRequest<DeleteRelatedPublicationCommandResponse>
{
}

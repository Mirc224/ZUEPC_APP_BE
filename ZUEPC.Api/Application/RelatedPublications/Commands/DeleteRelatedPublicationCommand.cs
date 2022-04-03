using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.RelatedPublications.Commands;

public class DeleteRelatedPublicationCommand : 
	DeleteModelCommandBase<long>, 
	IRequest<DeleteRelatedPublicationCommandResponse>
{
}

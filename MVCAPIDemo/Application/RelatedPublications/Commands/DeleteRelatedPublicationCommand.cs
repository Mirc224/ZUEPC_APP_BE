using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.RelatedPublications.Commands;

public class DeleteRelatedPublicationCommand : EPCDeleteBaseCommand, IRequest<DeleteRelatedPublicationCommandResponse>
{
}

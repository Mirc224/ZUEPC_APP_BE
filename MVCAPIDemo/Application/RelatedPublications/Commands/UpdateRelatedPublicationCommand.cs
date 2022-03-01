using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.RelatedPublications.Commands;

public class UpdateRelatedPublicationCommand : EPCUpdateBaseCommand, IRequest<UpdateRelatedPublicationCommandResponse>
{
	public long PublicationId { get; set; }
	public long RelatedPublicationId { get; set; }
	public string? RelationType { get; set; }
	public string? CitationCategory { get; set; }
}

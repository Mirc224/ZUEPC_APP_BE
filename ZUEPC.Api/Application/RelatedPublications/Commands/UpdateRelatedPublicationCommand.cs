using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.RelatedPublications.Commands;

public class UpdateRelatedPublicationCommand : EPCUpdateCommandBase, IRequest<UpdateRelatedPublicationCommandResponse>
{
	public long PublicationId { get; set; }
	public long RelatedPublicationId { get; set; }
	public string? RelationType { get; set; }
	public string? CitationCategory { get; set; }
}

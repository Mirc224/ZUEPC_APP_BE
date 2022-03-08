using ZUEPC.Application.Publications.Entities.Inputs.Common;

namespace ZUEPC.Application.RelatedPublications.Entities.Inputs.RelatedPublications;

public class RelatedPublicationBaseDto : PublicationPropertyBaseDto
{
	public long RelatedPublicationId { get; set; }
	public string? RelationType { get; set; }
	public string? CitationCategory { get; set; }
}

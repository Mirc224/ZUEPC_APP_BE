using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.RelatedPublications;

public class RelatedPublication : EPCBase
{
	public long PublicationId { get; set; }
	public long RelatedPublicationId { get; set; }
	public string? RelationType { get; set; }
	public string? CitationCategory { get; set; }
}

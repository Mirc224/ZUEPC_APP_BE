using ZUEPC.Base.ItemInterfaces;
using ZUEPC.EvidencePublication.Domain.Common;

namespace ZUEPC.EvidencePublication.Domain.RelatedPublications;

public class RelatedPublication : 
	EPCDomainBase,
	IPublicationRelated
{
	public long PublicationId { get; set; }
	public long RelatedPublicationId { get; set; }
	public string? RelationType { get; set; }
	public string? CitationCategory { get; set; }
}

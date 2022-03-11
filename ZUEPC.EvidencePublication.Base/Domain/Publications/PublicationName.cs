using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Domain.Common.Interfaces;

namespace ZUEPC.EvidencePublication.Base.Domain.Publications;

public class PublicationName : 
	EPCDomainBase, 
	IPublicationRelated
{
	public long PublicationId { get; set; }
	public string? Name { get; set; }
	public string? NameType { get; set; }
}

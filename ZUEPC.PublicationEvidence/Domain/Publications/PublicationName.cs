using ZUEPC.Base.Domain;
using ZUEPC.Base.ItemInterfaces;

namespace ZUEPC.EvidencePublication.Domain.Publications;

public class PublicationName : 
	EPCDomainBase, 
	IPublicationRelated
{
	public long PublicationId { get; set; }
	public string? Name { get; set; }
	public string? NameType { get; set; }
}

using ZUEPC.Base.Domain;
using ZUEPC.Base.ItemInterfaces;

namespace ZUEPC.EvidencePublication.Domain.Publications;

public class PublicationIdentifier : 
	EPCDomainBase, 
	IPublicationRelated
{
	public long PublicationId { get; set; }
	public string? IdentifierValue { get; set; }
	public string? IdentifierName { get; set; }
	public string? ISForm { get; set; }
}

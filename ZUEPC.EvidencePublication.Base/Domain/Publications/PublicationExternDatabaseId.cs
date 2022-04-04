using ZUEPC.Base.ItemInterfaces;
using ZUEPC.Base.Domain;

namespace ZUEPC.EvidencePublication.Domain.Publications;

public class PublicationExternDatabaseId : 
	EPCDomainBase,
	IPublicationRelated,
	IEPCItemWithExternIdentifier
{
	public long PublicationId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}

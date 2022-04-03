using ZUEPC.DataAccess.Interfaces;
using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Publications;

public class PublicationExternDatabaseId : 
	EPCDomainBase,
	IPublicationRelated,
	IEPCItemWithExternIdentifier
{
	public long PublicationId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}

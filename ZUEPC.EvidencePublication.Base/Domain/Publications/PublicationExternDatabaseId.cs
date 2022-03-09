using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Domain.Common.Interfaces;

namespace ZUEPC.EvidencePublication.Base.Domain.Publications;

public class PublicationExternDatabaseId 
	: EPCExternDatabaseIdBase,
	IPublicationRelated
{
	public long PublicationId { get; set; }
}

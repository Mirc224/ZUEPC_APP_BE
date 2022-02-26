using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Publications;

public class PublicationExternDatabaseId : EPCExternDatabaseIdBase
{
	public long PublicationId { get; set; }
}

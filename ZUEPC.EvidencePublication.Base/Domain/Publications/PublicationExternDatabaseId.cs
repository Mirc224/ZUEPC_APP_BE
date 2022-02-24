using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Publications;

public class PublicationExternDatabaseId : EPCBase
{
	public long PublicationId { get; set; }
	public string? ExternDatabaseId { get; set; }
}

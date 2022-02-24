using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Publications;

public class PublicationName : EPCBase
{
	public long PublicationId { get; set; }
	public string? Name { get; set; }
	public string? NameType { get; set; }
}

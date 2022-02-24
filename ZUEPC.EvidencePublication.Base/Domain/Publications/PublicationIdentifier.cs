using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Publications;

public class PublicationIdentifier : EPCBase
{
	public long PublicationId { get; set; }
	public string? PublicationIdentifierValue { get; set; }
	public string? IdentifierName { get; set; }
	public string? ISForm { get; set; }
}

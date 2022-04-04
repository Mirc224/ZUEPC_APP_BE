using ZUEPC.EvidencePublication.Domain.Common;

namespace ZUEPC.EvidencePublication.Domain.Publications;

public class Publication : EPCDomainBase
{
	public string? DocumentType { get; set; }
	public int? PublishYear { get; set; }
}


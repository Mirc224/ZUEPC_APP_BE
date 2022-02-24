using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Publications;

public class Publication : EPCBase
{
	public string? DocumentType { get; set; }
	public int? PublishYear { get; set; }
}


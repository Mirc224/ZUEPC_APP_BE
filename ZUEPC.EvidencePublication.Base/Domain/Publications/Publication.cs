using ZUEPC.Base.Domain;

namespace ZUEPC.EvidencePublication.Domain.Publications;

public class Publication : EPCDomainBase
{
	public int? PublishYear { get; set; }
	public string? DocumentType { get; set; }
}


using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Institutions;

public class Institution : EPCDomainBase
{
	public int? Level { get; set; }
	public string? InstitutionType { get; set; }
}

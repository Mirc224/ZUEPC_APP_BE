using ZUEPC.Base.Domain;

namespace ZUEPC.EvidencePublication.Domain.Institutions;

public class Institution : EPCDomainBase
{
	public int? Level { get; set; }
	public string? InstitutionType { get; set; }
}

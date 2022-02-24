using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Institutions;

public class InstitutionName : EPCBase
{
	public long InstitutionId { get; set; }
	public string? NameType { get; set; }
	public string? Name { get; set; }
}

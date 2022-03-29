using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Domain.Common.Interfaces;

namespace ZUEPC.EvidencePublication.Base.Domain.Institutions;

public class InstitutionName : EPCDomainBase, IInstitutionRelated
{
	public long InstitutionId { get; set; }
	public string? Name { get; set; }
	public string? NameType { get; set; }
}

using ZUEPC.DataAccess.Interfaces;
using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Institutions;

public class InstitutionName : 
	EPCDomainBase, 
	IInstitutionRelated
{
	public long InstitutionId { get; set; }
	public string? Name { get; set; }
	public string? NameType { get; set; }
}

using ZUEPC.Base.ItemInterfaces;
using ZUEPC.EvidencePublication.Domain.Common;

namespace ZUEPC.EvidencePublication.Domain.Institutions;

public class InstitutionName : 
	EPCDomainBase, 
	IInstitutionRelated
{
	public long InstitutionId { get; set; }
	public string? Name { get; set; }
	public string? NameType { get; set; }
}

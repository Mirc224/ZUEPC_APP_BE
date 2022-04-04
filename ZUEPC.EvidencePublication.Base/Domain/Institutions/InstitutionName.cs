using ZUEPC.Base.Domain;
using ZUEPC.Base.ItemInterfaces;

namespace ZUEPC.EvidencePublication.Domain.Institutions;

public class InstitutionName : 
	EPCDomainBase, 
	IInstitutionRelated
{
	public long InstitutionId { get; set; }
	public string? Name { get; set; }
	public string? NameType { get; set; }
}

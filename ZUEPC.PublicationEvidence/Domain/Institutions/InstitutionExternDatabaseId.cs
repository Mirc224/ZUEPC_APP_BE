using ZUEPC.Base.ItemInterfaces;
using ZUEPC.Base.Domain;

namespace ZUEPC.EvidencePublication.Domain.Institutions;

public class InstitutionExternDatabaseId : 
	EPCDomainBase, 
	IInstitutionRelated,
	IEPCItemWithExternIdentifier
{
	public long InstitutionId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}

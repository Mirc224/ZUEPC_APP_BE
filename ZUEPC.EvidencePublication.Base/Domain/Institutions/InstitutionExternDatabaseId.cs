using ZUEPC.DataAccess.Interfaces;
using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Institutions;

public class InstitutionExternDatabaseId : 
	EPCDomainBase, 
	IInstitutionRelated,
	IEPCItemWithExternIdentifier
{
	public long InstitutionId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}

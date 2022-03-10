using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Domain.Common.Interfaces;

namespace ZUEPC.EvidencePublication.Base.Domain.Institutions;

public class InstitutionExternDatabaseId : EPCExternDatabaseIdBase, IInstitutionRelated
{
	public long InstitutionId { get; set; }
}

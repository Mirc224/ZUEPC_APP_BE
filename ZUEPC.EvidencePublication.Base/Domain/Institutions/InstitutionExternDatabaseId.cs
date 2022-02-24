using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Institutions;

public class InstitutionExternDatabaseId : EPCBase
{
	public long InstitutionId { get; set; }
	public string ExternDbId { get; set; }
}

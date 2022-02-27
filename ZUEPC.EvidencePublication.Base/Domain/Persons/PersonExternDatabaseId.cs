using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Persons;

public class PersonExternDatabaseId : EPCExternDatabaseIdBase
{
	public long PersonId { get; set; }
}

using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Persons;

public class PersonExternDatabaseId : EPCBase
{
	public long PersonId { get; set; }
	public string ExternDatabaseId { get; set; }
}

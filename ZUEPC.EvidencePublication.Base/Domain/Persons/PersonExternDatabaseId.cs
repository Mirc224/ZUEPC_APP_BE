using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Domain.Common.Interfaces;

namespace ZUEPC.EvidencePublication.Base.Domain.Persons;

public class PersonExternDatabaseId : 
	EPCExternDatabaseIdBase,
	IPersonRelated
{
	public long PersonId { get; set; }
}

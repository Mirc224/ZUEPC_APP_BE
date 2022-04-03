using ZUEPC.DataAccess.Interfaces;
using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Persons;

public class PersonExternDatabaseId : 
	EPCDomainBase,
	IPersonRelated,
	IEPCItemWithExternIdentifier
{
	public long PersonId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}

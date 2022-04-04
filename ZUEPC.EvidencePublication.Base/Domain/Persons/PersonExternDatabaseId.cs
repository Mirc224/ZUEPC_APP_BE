using ZUEPC.Base.ItemInterfaces;
using ZUEPC.EvidencePublication.Domain.Common;

namespace ZUEPC.EvidencePublication.Domain.Persons;

public class PersonExternDatabaseId : 
	EPCDomainBase,
	IPersonRelated,
	IEPCItemWithExternIdentifier
{
	public long PersonId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}

using ZUEPC.Base.Domain;
using ZUEPC.Base.ItemInterfaces;

namespace ZUEPC.EvidencePublication.Domain.Persons;

public class PersonExternDatabaseId : 
	EPCDomainBase,
	IPersonRelated,
	IEPCItemWithExternIdentifier
{
	public long PersonId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}

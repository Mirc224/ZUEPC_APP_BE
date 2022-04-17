using ZUEPC.DataAccess.Attributes.ModelAttributes;
using ZUEPC.Base.ItemInterfaces;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Person;

public class PersonExternDatabaseIdModel : 
	EPCModelBase,
	IEPCItemWithExternIdentifier
{
	[ExcludeFromUpdate]
	public long PersonId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}

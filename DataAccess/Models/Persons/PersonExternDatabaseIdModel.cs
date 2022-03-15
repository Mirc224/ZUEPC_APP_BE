using ZUEPC.DataAccess.Attributes.ModelAttributes;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Person;

public class PersonExternDatabaseIdModel : EPCExternDatabaseIdBaseModel
{
	[ExcludeFromUpdate]
	public long PersonId { get; set; }
}

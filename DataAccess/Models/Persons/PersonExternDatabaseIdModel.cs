using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Person;

public class PersonExternDatabaseIdModel : EPCExternDatabaseIdBaseModel
{
	public long PersonId { get; set; }
}

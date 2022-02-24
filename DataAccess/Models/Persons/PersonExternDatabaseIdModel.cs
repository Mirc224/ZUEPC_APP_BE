using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Person;

public class PersonExternDatabaseIdModel : EPCBaseModel
{
	public long PersonId { get; set; }
	public string ExternDatabaseId { get; set; }
}

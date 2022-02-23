using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Person;

public class PersonNameModel : EPCBaseModel
{
	public long PersonId { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? NameType { get; set; }
}

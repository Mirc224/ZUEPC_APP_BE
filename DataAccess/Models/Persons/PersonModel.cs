using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Person;

public class PersonModel : EPCBaseModel
{
	public int? BirthYear { get; set; }
	public int? DeathYear { get; set; }
}

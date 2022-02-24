using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Persons;

public class Person : EPCBase
{
	public int? BirthYear { get; set; }
	public int? DeathYear { get; set; }
}

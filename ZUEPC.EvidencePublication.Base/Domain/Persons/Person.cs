using ZUEPC.EvidencePublication.Domain.Common;

namespace ZUEPC.EvidencePublication.Domain.Persons;

public class Person : EPCDomainBase
{
	public int? BirthYear { get; set; }
	public int? DeathYear { get; set; }
}

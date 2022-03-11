using ZUEPC.Common.Entities;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Entities.Details;

public class PersonDetails : DetailsBase
{
	public int? BirthYear { get; set; }
	public int? DeathYear { get; set; }
	public ICollection<PersonName>? Names { get; set; }
	public ICollection<PersonExternDatabaseId>? ExternDatabaseIds { get; set; }
}

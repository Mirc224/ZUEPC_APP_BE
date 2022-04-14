using ZUEPC.Base.Entities;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Entities.Details;

public class PersonDetails : ItemDetailsBase
{
	public int? BirthYear { get; set; }
	public int? DeathYear { get; set; }
	public IEnumerable<PersonName>? Names { get; set; }
	public IEnumerable<PersonExternDatabaseId>? ExternDatabaseIds { get; set; }
}

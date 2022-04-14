using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Entities.Previews;

public class PersonPreview
{
	public long Id { get; set; }
	public int? BirthYear { get; set; }
	public int? DeathYear { get; set; }
	public IEnumerable<PersonName>? Names { get; set; }
	public IEnumerable<PersonExternDatabaseId>? ExternDatabaseIds { get; set; }
}

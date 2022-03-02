using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Entities.Previews;

public class PersonPreview
{
	public long Id { get; set; }
	public int? BirthYear { get; set; }
	public int? DeathYear { get; set; }
	public ICollection<PersonName>? PersonNames { get; set; }
}

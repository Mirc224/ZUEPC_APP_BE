using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Persons;

public class PersonName : EPCBase
{
	public long PersonId { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? NameType { get; set; }
}

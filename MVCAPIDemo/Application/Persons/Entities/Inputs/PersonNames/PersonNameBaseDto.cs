using ZUEPC.Common.Entities.Inputs;

namespace ZUEPC.Application.Persons.Entities.Inputs.PersonNames;

public class PersonNameBaseDto : EPCBaseDto
{
	public long? PersonId { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? NameType { get; set; }
}
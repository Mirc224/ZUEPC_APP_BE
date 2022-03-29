using ZUEPC.Application.Persons.Entities.Inputs.Common;

namespace ZUEPC.Application.Persons.Entities.Inputs.PersonNames;

public class PersonNameBaseDto : PersonPropertyBaseDto
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? NameType { get; set; }
}
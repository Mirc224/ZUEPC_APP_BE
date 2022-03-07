using ZUEPC.Application.Persons.Entities.Inputs.Common;

namespace ZUEPC.Application.Persons.Entities.Inputs.PersonExternDatabaseIds;

public class PersonExternDatabaseIdBaseDto : PersonPropertyBaseDto
{
	public string ExternIdentifierValue { get; set; }
}
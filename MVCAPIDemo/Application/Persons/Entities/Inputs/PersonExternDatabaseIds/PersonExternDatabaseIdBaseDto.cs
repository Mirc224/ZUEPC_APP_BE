using ZUEPC.Common.Entities.Inputs;

namespace ZUEPC.Application.Persons.Entities.Inputs.PersonExternDatabaseIds;

public class PersonExternDatabaseIdBaseDto : EPCBaseDto
{
	public long? PersonId { get; set; }
	public string ExternIdentifierValue { get; set; }
}
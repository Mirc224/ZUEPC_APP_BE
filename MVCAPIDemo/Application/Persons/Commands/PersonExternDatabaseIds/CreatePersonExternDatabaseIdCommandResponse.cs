using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;

public class CreatePersonExternDatabaseIdCommandResponse : ResponseBase
{
	public PersonExternDatabaseId? CreatedPersonExternDatabaseId { get; set; }
}
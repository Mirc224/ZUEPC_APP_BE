using MediatR;

namespace ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;

public class DeletePersonExternDatabaseIdsByPersonIdCommand : IRequest<DeletePersonExternDatabaseIdsByPersonIdCommandResponse>
{
	public long PersonId { get; set; }
}

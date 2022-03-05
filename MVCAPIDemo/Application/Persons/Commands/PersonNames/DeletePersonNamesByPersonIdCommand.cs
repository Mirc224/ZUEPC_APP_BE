using MediatR;

namespace ZUEPC.Application.Persons.Commands.PersonNames;

public class DeletePersonNamesByPersonIdCommand : IRequest<DeletePersonNamesByPersonIdCommandResponse>
{
	public long PersonId { get; set; }
}

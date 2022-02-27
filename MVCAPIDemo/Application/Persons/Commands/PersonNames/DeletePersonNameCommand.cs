using MediatR;

namespace ZUEPC.Application.Persons.Commands.PersonNames;

public class DeletePersonNameCommand : IRequest<DeletePersonNameCommandResponse>
{
	public long PersonNameId { get; set; }
}

using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;

public class CreatePersonExternDatabaseIdCommand : EPCCreateCommandBase, IRequest<CreatePersonExternDatabaseIdCommandResponse>
{
	public long PersonId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}

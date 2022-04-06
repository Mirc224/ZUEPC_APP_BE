using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;

public class UpdatePersonExternDatabaseIdCommand : EPCUpdateCommandBase, IRequest<UpdatePersonExternDatabaseIdCommandResponse>
{
	public long PersonId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}

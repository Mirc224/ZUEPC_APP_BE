using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;

public class CreatePersonExternDatabaseIdCommand : EPCCreateCommandBase, IRequest<CreatePersonExternDatabaseIdCommandResponse>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public long PersonId { get; set; }
	public string ExternIdentifierValue { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}

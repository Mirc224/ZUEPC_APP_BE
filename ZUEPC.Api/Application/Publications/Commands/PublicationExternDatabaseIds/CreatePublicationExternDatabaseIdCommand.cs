using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;

public class CreatePublicationExternDatabaseIdCommand : EPCCreateCommandBase, IRequest<CreatePublicationExternDatabaseIdCommandResponse>
{
	public long PublicationId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}

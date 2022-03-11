using MediatR;

namespace ZUEPC.Api.Application.Publications.Commands.PublicationExternDatabaseIds;

public class DeletePublicationExternDatabaseIdsByPublicationIdCommand : IRequest<DeletePublicationExternDatabaseIdsByPublicationIdCommandResponse>
{
	public long PublicationId { get; set; }
}

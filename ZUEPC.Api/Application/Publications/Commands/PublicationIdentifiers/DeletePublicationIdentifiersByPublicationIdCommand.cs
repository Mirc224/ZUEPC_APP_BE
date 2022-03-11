using MediatR;

namespace ZUEPC.Api.Application.Publications.Commands.PublicationIdentifiers;

public class DeletePublicationIdentifiersByPublicationIdCommand : IRequest<DeletePublicationIdentifiersByPublicationIdCommandResponse>
{
	public long PublicationId { get; set; }
}

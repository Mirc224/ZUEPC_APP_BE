using MediatR;

namespace ZUEPC.Api.Application.PublicationAuthors.Commands;

public class DeletePublicationAuthorsByPublicationIdCommand :
	IRequest<DeletePublicationAuthorsByPublicationIdCommandResponse>
{
	public long PublicationId { get; set; }
}

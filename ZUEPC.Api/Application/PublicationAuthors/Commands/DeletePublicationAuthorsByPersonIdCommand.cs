using MediatR;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class DeletePublicationAuthorsByPersonIdCommand : IRequest<DeletePublicationAuthorsByPersonIdCommandResponse>
{
	public long PersonId { get; set; }
}

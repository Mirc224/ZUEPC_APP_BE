using MediatR;

namespace ZUEPC.Api.Application.RelatedPublications.Commands;

public class DeleteRelatedPublicationsByPublicationIdCommand : IRequest<DeleteRelatedPublicationsByPublicationIdCommandResponse>
{
	public long PublicationId { get; set; }
}

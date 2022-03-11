using MediatR;

namespace ZUEPC.Api.Application.RelatedPublications.Commands;

public class DeleteRelatedPublicationsByRelatedPublicationIdCommand : IRequest<DeleteRelatedPublicationsByRelatedPublicationIdCommandResponse>
{
	public long RelatedPublicationId { get; set; }
}

using MediatR;

namespace ZUEPC.Application.PublicationAuthors.Queries;

public class GetPublicationPublicationAuthorsQuery : IRequest<GetPublicationPublicationAuthorsQueryResponse>
{
	public long PublicationId { get; set; }
}

using MediatR;

namespace ZUEPC.Application.PublicationAuthors.Queries;

public class GetPublicationAuthorsQuery : IRequest<GetPublicationAuthorsQueryResponse>
{
	public long PublicationId { get; set; }
}

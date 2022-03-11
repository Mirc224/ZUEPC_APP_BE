using MediatR;

namespace ZUEPC.Application.PublicationAuthors.Queries.Previews;

public class GetPublicationAuthorsPreviewsQuery : IRequest<GetPublicationAuthorsPreviewsQueryResponse>
{
	public long PublicationId { get; set; }
}

using MediatR;

namespace ZUEPC.Application.PublicationAuthors.Queries.Details;

public class GetPublicationAuthorDetailsQuery : IRequest<GetPublicationAuthorDetailsQueryResponse>
{
	public long PublicationId { get; set; }
}

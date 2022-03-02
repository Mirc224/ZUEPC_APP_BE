using MediatR;

namespace ZUEPC.Application.Publications.Queries.Publications.Details;

public class GetPublicationDetailsQuery : IRequest<GetPublicationDetailsQueryResponse>
{
	public long PublicationId { get; set; }
}

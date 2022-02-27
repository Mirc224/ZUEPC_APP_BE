using MediatR;

namespace ZUEPC.Application.Publications.Queries.PublicationNames;

public class GetPublicationNamesQuery : IRequest<GetPublicationNamesQueryResponse>
{
	public long PublicationId { get; set; }
}

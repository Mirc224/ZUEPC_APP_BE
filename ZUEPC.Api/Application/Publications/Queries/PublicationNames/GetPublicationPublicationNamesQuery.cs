using MediatR;

namespace ZUEPC.Application.Publications.Queries.PublicationNames;

public class GetPublicationPublicationNamesQuery : IRequest<GetPublicationPublicationNamesQueryResponse>
{
	public long PublicationId { get; set; }
}

using MediatR;

namespace ZUEPC.Application.Publications.Queries.PublicationNames;

public class GetAllPublicationNamesQuery : IRequest<GetAllPublicationNamesQueryResponse>
{
	public long PublicationId { get; set; }
}

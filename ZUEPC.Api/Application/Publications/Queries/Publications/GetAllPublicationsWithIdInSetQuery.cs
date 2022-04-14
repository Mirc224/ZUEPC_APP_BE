using MediatR;

namespace ZUEPC.Api.Application.Publications.Queries.Publications;

public class GetAllPublicationsWithIdInSetQuery : IRequest<GetAllPublicationsWithIdInSetQueryResponse>
{
	public IEnumerable<long> PublicationIds { get; set; }
}

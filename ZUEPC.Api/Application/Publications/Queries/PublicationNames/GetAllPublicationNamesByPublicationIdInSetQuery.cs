using MediatR;

namespace ZUEPC.Api.Application.Publications.Queries.PublicationNames;

public class GetAllPublicationNamesByPublicationIdInSetQuery : IRequest<GetAllPublicationNamesByPublicationIdInSetQueryResponse>
{
	public IEnumerable<long> PublicationIds { get; set; }
}

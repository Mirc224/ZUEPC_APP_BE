using MediatR;

namespace ZUEPC.Api.Application.Publications.Queries.Publications.Details;

public class GetAllPublicationsDetailsDataForPublicationIdsInSetQuery :
	IRequest<GetAllPublicationsDetailsDataForPublicationIdsInSetQueryResponse>
{
	public IEnumerable<long> PublicationIds { get; set; }
}
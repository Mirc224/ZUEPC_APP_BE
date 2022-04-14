using MediatR;

namespace ZUEPC.Api.Application.PublicationActivities.Queries;

public class GetAllPublicationActivitiesByPublicationIdInSetQuery : IRequest<GetAllPublicationActivitiesByPublicationIdInSetQueryResponse>
{
	public IEnumerable<long> PublicationIds { get; set; }
}

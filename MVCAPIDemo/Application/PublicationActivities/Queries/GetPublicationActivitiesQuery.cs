using MediatR;

namespace ZUEPC.Application.PublicationActivities.Queries;

public class GetPublicationActivitiesQuery : IRequest<GetPublicationActivitiesQueryResponse>
{
	public long PublicationId { get; set; }
}

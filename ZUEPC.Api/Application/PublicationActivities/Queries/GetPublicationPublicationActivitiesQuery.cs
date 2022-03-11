using MediatR;

namespace ZUEPC.Application.PublicationActivities.Queries;

public class GetPublicationPublicationActivitiesQuery : IRequest<GetPublicationPublicationActivitiesQueryResponse>
{
	public long PublicationId { get; set; }
}

using MediatR;

namespace ZUEPC.Application.PublicationActivities.Queries;

public class GetPublicationActivitiesCommand : IRequest<GetPublicationActivitiesCommandResponse>
{
	public long PublicationId { get; set; }
}

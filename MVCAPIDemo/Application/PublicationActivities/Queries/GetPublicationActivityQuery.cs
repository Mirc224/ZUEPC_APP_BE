using MediatR;

namespace ZUEPC.Application.PublicationActivities.Queries;

public class GetPublicationActivityQuery : IRequest<GetPublicationActivityQueryResponse>
{
	public long PublicationActivityRecordId { get; set; }
}

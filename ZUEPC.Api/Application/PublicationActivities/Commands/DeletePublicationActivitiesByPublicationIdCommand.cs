using MediatR;

namespace ZUEPC.Api.Application.PublicationActivities.Commands;

public class DeletePublicationActivitiesByPublicationIdCommand :
	IRequest<DeletePublicationActivitiesByPublicationIdCommandResponse>
{
	public long PublicationId { get; set; }
}

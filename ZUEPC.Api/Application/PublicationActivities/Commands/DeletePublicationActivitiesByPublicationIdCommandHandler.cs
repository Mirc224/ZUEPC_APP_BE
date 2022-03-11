using MediatR;
using ZUEPC.DataAccess.Data.PublicationActivities;

namespace ZUEPC.Api.Application.PublicationActivities.Commands;

public class DeletePublicationActivitiesByPublicationIdCommandHandler :
	IRequestHandler<DeletePublicationActivitiesByPublicationIdCommand, DeletePublicationActivitiesByPublicationIdCommandResponse>
{
	private readonly IPublicationActivityData _repository;

	public DeletePublicationActivitiesByPublicationIdCommandHandler(IPublicationActivityData repository)
	{
		_repository = repository;
	}
	public async Task<DeletePublicationActivitiesByPublicationIdCommandResponse> Handle(DeletePublicationActivitiesByPublicationIdCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeletePublicationActivityByPublicationIdAsync(request.PublicationId);
		return new() { Success = true };
	}
}

using MediatR;
using ZUEPC.DataAccess.Data.PublicationActivities;

namespace ZUEPC.Application.PublicationActivities.Commands;

public class DeletePublicationActivityCommandHandler : IRequestHandler<DeletePublicationActivityCommand, DeletePublicationActivityCommandResponse>
{
	private readonly IPublicationActivityData _repository;

	public DeletePublicationActivityCommandHandler(IPublicationActivityData repository)
	{
		_repository = repository;
	}

	public async Task<DeletePublicationActivityCommandResponse> Handle(DeletePublicationActivityCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeletePublicationActivityByIdAsync(request.Id);
		return new() { Success = rowsDeleted == 1 };
	}
}

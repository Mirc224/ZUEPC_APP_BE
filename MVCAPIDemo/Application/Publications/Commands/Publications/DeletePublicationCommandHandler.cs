using MediatR;
using ZUEPC.DataAccess.Data.Publications;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class DeletePublicationCommandHandler : 
	IRequestHandler<
		DeletePublicationCommand, 
		DeletePublicationCommandResponse>
{
	private readonly IPublicationData _repository;

	public DeletePublicationCommandHandler(IPublicationData repository)
	{
		_repository = repository;
	}
	public async Task<DeletePublicationCommandResponse> Handle(DeletePublicationCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeletePublicationByIdAsync(request.PublicationId);

		return new() { Success = rowsDeleted == 1 };
	}
}

using MediatR;
using ZUEPC.DataAccess.Data.Publications;

namespace ZUEPC.Application.Publications.Commands.PublicationIdentifiers;

public class DeletePublicationIdentifierCommandHandler : IRequestHandler<DeletePublicationIdentifierCommand, DeletePublicationIdentifierCommandResponse>
{
	private readonly IPublicationIdentifierData _repository;

	public DeletePublicationIdentifierCommandHandler(IPublicationIdentifierData repository)
	{
		_repository = repository;
	}
	public async Task<DeletePublicationIdentifierCommandResponse> Handle(DeletePublicationIdentifierCommand request, CancellationToken cancellationToken)
	{
		var identifierModel = await _repository.GetModelByIdAsync(request.Id);
		if (identifierModel is null)
		{
			return new() { Success = false };
		}

		int rowsDeleted = await _repository.DeleteModelByIdAsync(request.Id);
		return new() { Success = rowsDeleted > 0 };
	}
}

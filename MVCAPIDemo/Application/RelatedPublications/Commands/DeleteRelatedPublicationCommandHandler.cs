using MediatR;
using ZUEPC.DataAccess.Data.RelatedPublications;

namespace ZUEPC.Application.RelatedPublications.Commands;

public class DeleteRelatedPublicationCommandHandler : IRequestHandler<DeleteRelatedPublicationCommand, DeleteRelatedPublicationCommandResponse>
{
	private readonly IRelatedPublicationData _repository;

	public DeleteRelatedPublicationCommandHandler(IRelatedPublicationData repository)
	{
		_repository = repository;
	}
	public async Task<DeleteRelatedPublicationCommandResponse> Handle(DeleteRelatedPublicationCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeleteRelatedPublicationByIdAsync(request.Id);

		return new() { Success = rowsDeleted == 1 };
	}
}

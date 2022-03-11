using MediatR;
using ZUEPC.DataAccess.Data.RelatedPublications;

namespace ZUEPC.Api.Application.RelatedPublications.Commands;

public class DeleteRelatedPublicationsByPublicationIdCommandHandler :
	IRequestHandler<DeleteRelatedPublicationsByPublicationIdCommand, DeleteRelatedPublicationsByPublicationIdCommandResponse>
{
	private readonly IRelatedPublicationData _repository;

	public DeleteRelatedPublicationsByPublicationIdCommandHandler(IRelatedPublicationData repository)
	{
		_repository = repository;
	}
	public async Task<DeleteRelatedPublicationsByPublicationIdCommandResponse> Handle(DeleteRelatedPublicationsByPublicationIdCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeleteRelatedPublicationsByPublicationIdAsync(request.PublicationId);
		return new() { Success = true };
	}
}

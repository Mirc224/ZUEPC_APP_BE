using MediatR;
using ZUEPC.DataAccess.Data.RelatedPublications;

namespace ZUEPC.Api.Application.RelatedPublications.Commands;

public class DeleteRelatedPublicationsByRelatedPublicationIdCommandHandler :
	IRequestHandler<DeleteRelatedPublicationsByRelatedPublicationIdCommand, DeleteRelatedPublicationsByRelatedPublicationIdCommandResponse>
{
	private readonly IRelatedPublicationData _repository;

	public DeleteRelatedPublicationsByRelatedPublicationIdCommandHandler(IRelatedPublicationData repository)
	{
		_repository = repository;
	}
	public async Task<DeleteRelatedPublicationsByRelatedPublicationIdCommandResponse> Handle(DeleteRelatedPublicationsByRelatedPublicationIdCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeleteRelatedPublicationsByRelatedPublicationIdAsync(request.RelatedPublicationId);
		return new() { Success = true };
	}
}

using MediatR;
using ZUEPC.DataAccess.Data.PublicationAuthors;

namespace ZUEPC.Api.Application.PublicationAuthors.Commands;

public class DeletePublicationAuthorsByPublicationIdCommandHandler :
	IRequestHandler<
		DeletePublicationAuthorsByPublicationIdCommand,
		DeletePublicationAuthorsByPublicationIdCommandResponse>
{
	private readonly IPublicationAuthorData _repository;

	public DeletePublicationAuthorsByPublicationIdCommandHandler(IPublicationAuthorData repository)
	{
		_repository = repository;
	}
	public async Task<DeletePublicationAuthorsByPublicationIdCommandResponse> Handle(DeletePublicationAuthorsByPublicationIdCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeletePublicationAuthorsByPublicationIdAsync(request.PublicationId);
		return new() { Success = true };
	}
}

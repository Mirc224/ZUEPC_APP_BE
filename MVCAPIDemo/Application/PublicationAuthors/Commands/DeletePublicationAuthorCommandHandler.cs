using MediatR;
using ZUEPC.DataAccess.Data.PublicationAuthors;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class DeletePublicationAuthorCommandHandler : IRequestHandler<DeletePublicationAuthorCommand, DeletePublicationAuthorCommandResponse>
{
	private readonly IPublicationAuthorData _repository;

	public DeletePublicationAuthorCommandHandler(IPublicationAuthorData repository)
	{
		_repository = repository;
	}

	public async Task<DeletePublicationAuthorCommandResponse> Handle(DeletePublicationAuthorCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeleteModelByIdAsync(request.Id);
		return new() { Success = rowsDeleted == 1 };
	}
}

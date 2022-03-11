using MediatR;
using ZUEPC.DataAccess.Data.PublicationAuthors;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class DeletePublicationAuthorsByPersonIdCommandHandler : 
	IRequestHandler<
		DeletePublicationAuthorsByPersonIdCommand, 
		DeletePublicationAuthorsByPersonIdCommandResponse>
{
	private readonly IPublicationAuthorData _repository;

	public DeletePublicationAuthorsByPersonIdCommandHandler(IPublicationAuthorData repository)
	{
		_repository = repository;
	}

	public async Task<DeletePublicationAuthorsByPersonIdCommandResponse> Handle(DeletePublicationAuthorsByPersonIdCommand request, CancellationToken cancellationToken)
	{
		await _repository.DeletePublicationAuthorsByPersonIdAsync(request.PersonId);
		return new() { Success = true };
	}
}

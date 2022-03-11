using MediatR;
using ZUEPC.DataAccess.Data.Publications;

namespace ZUEPC.Api.Application.Publications.Commands.PublicationIdentifiers;

public class DeletePublicationIdentifiersByPublicationIdCommandHandler
	: IRequestHandler<DeletePublicationIdentifiersByPublicationIdCommand, DeletePublicationIdentifiersByPublicationIdCommandResponse>
{
	private readonly IPublicationIdentifierData _repository;

	public DeletePublicationIdentifiersByPublicationIdCommandHandler(IPublicationIdentifierData repository)
	{
		_repository = repository;
	}

	public async Task<DeletePublicationIdentifiersByPublicationIdCommandResponse> Handle(DeletePublicationIdentifiersByPublicationIdCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeletePublicationIdentifiersByPublicationIdAsync(request.PublicationId);
		return new() { Success = true };
	}
}

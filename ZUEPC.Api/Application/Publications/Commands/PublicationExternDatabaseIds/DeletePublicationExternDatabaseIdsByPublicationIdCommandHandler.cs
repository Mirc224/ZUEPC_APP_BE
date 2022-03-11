using MediatR;
using ZUEPC.DataAccess.Data.Publications;

namespace ZUEPC.Api.Application.Publications.Commands.PublicationExternDatabaseIds;

public class DeletePublicationExternDatabaseIdsByPublicationIdCommandHandler :
	IRequestHandler<
		DeletePublicationExternDatabaseIdsByPublicationIdCommand,
		DeletePublicationExternDatabaseIdsByPublicationIdCommandResponse>
{
	private readonly IPublicationExternDatabaseIdData _repository;

	public DeletePublicationExternDatabaseIdsByPublicationIdCommandHandler(IPublicationExternDatabaseIdData repository)
	{
		_repository = repository;
	}
	public async Task<DeletePublicationExternDatabaseIdsByPublicationIdCommandResponse> Handle(DeletePublicationExternDatabaseIdsByPublicationIdCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeletePublicationExternDbIdsByPublicationIdAsync(request.PublicationId);
		return new() { Success = true };
	}
}

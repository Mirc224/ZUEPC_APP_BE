using MediatR;
using ZUEPC.DataAccess.Data.Persons;

namespace ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;

public class DeletePersonExternDatabaseIdCommandHandler : IRequestHandler<DeletePersonExternDatabaseIdCommand, DeletePersonExternDatabaseIdCommandResponse>
{
	private readonly IPersonExternDatabaseIdData _repository;

	public DeletePersonExternDatabaseIdCommandHandler(IPersonExternDatabaseIdData repository)
	{
		_repository = repository;
	}
	public async Task<DeletePersonExternDatabaseIdCommandResponse> Handle(DeletePersonExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeletePersonExternDatabaseIdByIdAsync(request.Id);
		return new() { Success = rowsDeleted > 0 };
	}
}

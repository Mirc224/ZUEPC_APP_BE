using MediatR;
using ZUEPC.DataAccess.Data.Persons;

namespace ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;

public class DeletePersonExternDatabaseIdsByPersonIdCommandHandler : IRequestHandler<DeletePersonExternDatabaseIdsByPersonIdCommand, DeletePersonExternDatabaseIdsByPersonIdCommandResponse>
{
	private readonly IPersonExternDatabaseIdData _repository;

	public DeletePersonExternDatabaseIdsByPersonIdCommandHandler(IPersonExternDatabaseIdData repository)
	{
		_repository = repository;
	}
	public async Task<DeletePersonExternDatabaseIdsByPersonIdCommandResponse> Handle(DeletePersonExternDatabaseIdsByPersonIdCommand request, CancellationToken cancellationToken)
	{
		await _repository.DeletePersonExternDatabaseIdsByPersonIdAsync(request.PersonId);
		return new() { Success = true };
	}
}

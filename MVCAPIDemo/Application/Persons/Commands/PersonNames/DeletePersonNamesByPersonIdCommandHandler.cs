using MediatR;
using ZUEPC.DataAccess.Data.Persons;

namespace ZUEPC.Application.Persons.Commands.PersonNames;

public class DeletePersonNamesByPersonIdCommandHandler : IRequestHandler<DeletePersonNamesByPersonIdCommand, DeletePersonNamesByPersonIdCommandResponse>
{
	private readonly IPersonNameData _repository;

	public DeletePersonNamesByPersonIdCommandHandler(IPersonNameData repository)
	{
		_repository = repository;
	}
	public async Task<DeletePersonNamesByPersonIdCommandResponse> Handle(DeletePersonNamesByPersonIdCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeletePersonNameByPersonIdAsync(request.PersonId);
		return new() { Success = true };
	}
}

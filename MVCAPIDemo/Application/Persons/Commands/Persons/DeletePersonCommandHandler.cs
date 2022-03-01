using MediatR;
using ZUEPC.DataAccess.Data.Persons;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, DeletePersonCommandResponse>
{
	private readonly IPersonData _repository;

	public DeletePersonCommandHandler(IPersonData repository)
	{
		_repository = repository;
	}
	public async Task<DeletePersonCommandResponse> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeletePersonByIdAsync(request.Id);

		return new() { Success = rowsDeleted == 1 };
	}
}

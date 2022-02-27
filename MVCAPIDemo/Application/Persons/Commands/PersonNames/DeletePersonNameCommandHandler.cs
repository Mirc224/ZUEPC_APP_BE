using MediatR;
using ZUEPC.DataAccess.Data.Persons;

namespace ZUEPC.Application.Persons.Commands.PersonNames;

public class DeletePersonNameCommandHandler : IRequestHandler<DeletePersonNameCommand, DeletePersonNameCommandResponse>
{
	private readonly IPersonNameData _repository;

	public DeletePersonNameCommandHandler(IPersonNameData repository)
	{
		_repository = repository;
	}

	public async Task<DeletePersonNameCommandResponse> Handle(DeletePersonNameCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeletePersonNameByIdAsync(request.PersonNameId);

		return new() { Success = rowsDeleted == 1 };
	}
}

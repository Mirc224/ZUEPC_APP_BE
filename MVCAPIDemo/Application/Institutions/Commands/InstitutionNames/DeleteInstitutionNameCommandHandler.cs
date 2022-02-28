using MediatR;
using ZUEPC.DataAccess.Data.Institutions;

namespace ZUEPC.Application.Institutions.Commands.InstitutionNames;

public class DeleteInstitutionNameCommandHandler :
	IRequestHandler<
		DeleteInstitutionNameCommand,
		DeleteInstitutionNameCommandResponse>
{
	private readonly IInstitutionNameData _repository;

	public DeleteInstitutionNameCommandHandler(IInstitutionNameData repository)
	{
		_repository = repository;
	}
	public async Task<DeleteInstitutionNameCommandResponse> Handle(DeleteInstitutionNameCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeleteInstitutionNameByIdAsync(request.Id);
		return new() { Success = rowsDeleted == 1 };
	}
}

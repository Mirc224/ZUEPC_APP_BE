using MediatR;
using ZUEPC.DataAccess.Data.Institutions;

namespace ZUEPC.Application.Institutions.Commands.InstitutionNames;

public class DeleteInstitutionNamesByInstitutionIdCommandHandler :
	IRequestHandler<DeleteInstitutionNamesByInstitutionIdCommand, DeleteInstitutionNamesByInstitutionIdCommandResponse>
{
	private readonly IInstitutionNameData _repository;

	public DeleteInstitutionNamesByInstitutionIdCommandHandler(IInstitutionNameData repository)
	{
		_repository = repository;
	}
	public async Task<DeleteInstitutionNamesByInstitutionIdCommandResponse> Handle(DeleteInstitutionNamesByInstitutionIdCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeleteInstitutionNamesByInstitutionIdAsync(request.InstitutionId);
		if(rowsDeleted == 0)
		{
			return new() { Success = false };
		}
		return new() { Success = true};
	}
}

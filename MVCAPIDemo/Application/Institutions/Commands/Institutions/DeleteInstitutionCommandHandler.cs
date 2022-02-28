using MediatR;
using ZUEPC.DataAccess.Data.Institutions;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class DeleteInstitutionCommandHandler : IRequestHandler<DeleteInstitutionCommand, DeleteInstitutionCommandResponse>
{
	private readonly IInstitutionData _repository;

	public DeleteInstitutionCommandHandler(IInstitutionData repository)
	{
		_repository = repository;
	}
	public async Task<DeleteInstitutionCommandResponse> Handle(DeleteInstitutionCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeleteInstitutionByIdAsync(request.InstitutionId);
		return new() { Success = rowsDeleted == 1 };
	}
}

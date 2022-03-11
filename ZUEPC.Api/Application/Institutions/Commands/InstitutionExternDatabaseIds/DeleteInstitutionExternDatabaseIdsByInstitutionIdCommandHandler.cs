using MediatR;
using ZUEPC.DataAccess.Data.Institutions;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class DeleteInstitutionExternDatabaseIdsByInstitutionIdCommandHandler :
	IRequestHandler<DeleteInstitutionExternDatabaseIdsByInstitutionIdCommand, DeleteInstitutionExternDatabaseIdsByInstitutionIdCommandResponse>
{
	private readonly IInstitutionExternDatabaseIdData _repository;

	public DeleteInstitutionExternDatabaseIdsByInstitutionIdCommandHandler(IInstitutionExternDatabaseIdData repository)
	{
		_repository = repository;
	}
	public async Task<DeleteInstitutionExternDatabaseIdsByInstitutionIdCommandResponse> Handle(
		DeleteInstitutionExternDatabaseIdsByInstitutionIdCommand request,
		CancellationToken cancellationToken)
	{
		await _repository.DeleteInstitutionExternDatabaseIdsByInstitutionIdAsync(request.InstitutionId);
		return new() { Success = true };
	}
}

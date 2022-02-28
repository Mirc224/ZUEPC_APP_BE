using MediatR;
using ZUEPC.DataAccess.Data.Institutions;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class DeleteInstitutionExternDatabaseIdsCommandHandler : 
	IRequestHandler<
		DeleteInstitutionExternDatabaseIdsCommand, 
		DeleteInstitutionExternDatabaseIdsCommandResponse>
{
	private readonly IInstitutionExternDatabaseIdData _repository;

	public DeleteInstitutionExternDatabaseIdsCommandHandler(IInstitutionExternDatabaseIdData repository)
	{
		_repository = repository;
	}

	public async Task<DeleteInstitutionExternDatabaseIdsCommandResponse> Handle(DeleteInstitutionExternDatabaseIdsCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeleteInstitutionExternDatabaseIdByIdAsync(request.Id);
		return new() { Success = rowsDeleted == 1};
	}
}

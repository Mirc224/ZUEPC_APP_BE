﻿using MediatR;
using ZUEPC.DataAccess.Data.Institutions;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class DeleteInstitutionExternDatabaseIdCommandHandler : 
	IRequestHandler<
		DeleteInstitutionExternDatabaseIdCommand, 
		DeleteInstitutionExternDatabaseIdCommandResponse>
{
	private readonly IInstitutionExternDatabaseIdData _repository;

	public DeleteInstitutionExternDatabaseIdCommandHandler(IInstitutionExternDatabaseIdData repository)
	{
		_repository = repository;
	}

	public async Task<DeleteInstitutionExternDatabaseIdCommandResponse> Handle(DeleteInstitutionExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeleteInstitutionExternDatabaseIdByIdAsync(request.Id);
		return new() { Success = rowsDeleted == 1};
	}
}

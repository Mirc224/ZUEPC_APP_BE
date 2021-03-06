using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class DeleteInstitutionExternDatabaseIdCommandHandler :
	DeleteModelWithIdCommandHandlerBase<InstitutionExternDatabaseIdModel, long>,
	IRequestHandler<
		DeleteInstitutionExternDatabaseIdCommand, 
		DeleteInstitutionExternDatabaseIdCommandResponse>
{
	public DeleteInstitutionExternDatabaseIdCommandHandler(IInstitutionExternDatabaseIdData repository)
		:base(repository) {}

	public async Task<DeleteInstitutionExternDatabaseIdCommandResponse> Handle(DeleteInstitutionExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		return await ProcessDeleteCommandAsync
			<DeleteInstitutionExternDatabaseIdCommand, DeleteInstitutionExternDatabaseIdCommandResponse>(request);
	}
}

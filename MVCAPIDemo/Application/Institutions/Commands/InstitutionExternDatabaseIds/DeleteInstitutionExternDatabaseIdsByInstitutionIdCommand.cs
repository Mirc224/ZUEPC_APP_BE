using MediatR;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class DeleteInstitutionExternDatabaseIdsByInstitutionIdCommand
	: IRequest<DeleteInstitutionExternDatabaseIdsByInstitutionIdCommandResponse>
{
	public long InstitutionId { get; set; }
}

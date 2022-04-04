using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class UpdateInstitutionExternDatabaseIdCommand : EPCUpdateCommandBase, IRequest<UpdateInstitutionExternDatabaseIdCommandResponse>
{
	public long InstitutionId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}

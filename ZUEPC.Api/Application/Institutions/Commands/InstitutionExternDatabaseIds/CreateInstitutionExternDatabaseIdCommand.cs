using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class CreateInstitutionExternDatabaseIdCommand : EPCCreateCommandBase, IRequest<CreateInstitutionExternDatabaseIdCommandResponse>
{
	public long InstitutionId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}

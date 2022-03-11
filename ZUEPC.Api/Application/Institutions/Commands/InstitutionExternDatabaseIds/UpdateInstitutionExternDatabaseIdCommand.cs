using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class UpdateInstitutionExternDatabaseIdCommand : EPCUpdateCommandBase, IRequest<UpdateInstitutionExternDatabaseIdCommandResponse>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public long InstitutionId { get; set; }
	public string ExternIdentifierValue { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}

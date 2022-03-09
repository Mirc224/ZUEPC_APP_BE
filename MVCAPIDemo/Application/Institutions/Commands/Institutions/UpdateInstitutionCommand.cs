using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class UpdateInstitutionCommand : EPCUpdateCommandBase, IRequest<UpdateInstitutionCommandResponse>
{
	public int? Level { get; set; }
	public string? InstititutionType { get; set; }
}

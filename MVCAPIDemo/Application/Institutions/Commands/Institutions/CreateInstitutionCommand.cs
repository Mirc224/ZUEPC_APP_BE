using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class CreateInstitutionCommand : EPCCreateBaseCommand, IRequest<CreateInstitutionCommandResponse>
{
	public int? Level { get; set; }
	public string? InstititutionType { get; set; }
}

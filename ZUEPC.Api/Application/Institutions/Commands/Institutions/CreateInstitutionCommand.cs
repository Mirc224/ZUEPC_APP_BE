using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class CreateInstitutionCommand : EPCCreateCommandBase, IRequest<CreateInstitutionCommandResponse>
{
	public int? Level { get; set; }
	public string? InstititutionType { get; set; }
}

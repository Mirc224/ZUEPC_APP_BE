using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.InstitutionNames;

public class UpdateInstitutionNameCommand : EPCUpdateCommandBase, IRequest<UpdateInstitutionNameCommandResponse>
{
	public long InstitutionId { get; set; }
	public string? NameType { get; set; }
	public string? Name { get; set; }
}

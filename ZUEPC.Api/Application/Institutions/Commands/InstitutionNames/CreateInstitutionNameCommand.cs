using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.InstitutionNames;

public class CreateInstitutionNameCommand : EPCCreateCommandBase, IRequest<CreateInstitutionNameCommandResponse>
{
	public long InstitutionId { get; set; }
	public string? NameType { get; set; }
	public string? Name { get; set; }
}

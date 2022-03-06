using MediatR;

namespace ZUEPC.Application.Institutions.Commands.InstitutionNames;

public class DeleteInstitutionNamesByInstitutionIdCommand : 
	IRequest<DeleteInstitutionNamesByInstitutionIdCommandResponse>
{
	public long InstitutionId { get; set; }
}

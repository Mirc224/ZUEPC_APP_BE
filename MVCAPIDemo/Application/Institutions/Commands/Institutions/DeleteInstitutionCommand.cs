using MediatR;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class DeleteInstitutionCommand : IRequest<DeleteInstitutionCommandResponse>
{
	public long InstitutionId { get; set; }
}

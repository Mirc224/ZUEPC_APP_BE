using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class DeleteInstitutionCommand : 
	EPCDeleteCommandBase<long>,
	IRequest<DeleteInstitutionCommandResponse>
{
}

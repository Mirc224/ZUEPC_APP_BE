using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class DeleteInstitutionCommand : 
	EPCDeleteModelCommandBase<long>,
	IRequest<DeleteInstitutionCommandResponse>
{
}

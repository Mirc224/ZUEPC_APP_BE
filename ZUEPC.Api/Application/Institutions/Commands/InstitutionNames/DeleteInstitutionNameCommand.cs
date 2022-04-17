using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.InstitutionNames;

public class DeleteInstitutionNameCommand : 
	EPCDeleteCommandBase<long>,
	IRequest<DeleteInstitutionNameCommandResponse>
{
}

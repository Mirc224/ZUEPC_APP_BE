using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class DeleteInstitutionExternDatabaseIdCommand 
	: EPCDeleteCommandBase<long>, 
	IRequest<DeleteInstitutionExternDatabaseIdCommandResponse>
{
}

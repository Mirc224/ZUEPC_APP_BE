using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class DeleteInstitutionExternDatabaseIdCommand 
	: EPCDeleteModelCommandBase<long>, 
	IRequest<DeleteInstitutionExternDatabaseIdCommandResponse>
{
}

using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class DeleteInstitutionExternDatabaseIdCommand 
	: DeleteModelCommandBase<long>, 
	IRequest<DeleteInstitutionExternDatabaseIdCommandResponse>
{
}

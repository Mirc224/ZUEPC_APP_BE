using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class DeleteInstitutionExternDatabaseIdCommand 
	: DeleteModelCommandBase, 
	IRequest<DeleteInstitutionExternDatabaseIdCommandResponse>
{
}

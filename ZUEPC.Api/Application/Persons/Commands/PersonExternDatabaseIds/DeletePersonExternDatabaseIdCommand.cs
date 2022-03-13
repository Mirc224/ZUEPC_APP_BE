using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;

public class DeletePersonExternDatabaseIdCommand : DeleteModelCommandBase, IRequest<DeletePersonExternDatabaseIdCommandResponse>
{
}

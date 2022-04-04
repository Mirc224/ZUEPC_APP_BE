using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;

public class DeletePersonExternDatabaseIdCommand : 
	EPCDeleteModelCommandBase<long>, 
	IRequest<DeletePersonExternDatabaseIdCommandResponse>
{
}

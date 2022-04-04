using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;

public class DeletePersonExternDatabaseIdCommand : 
	DeleteModelCommandBase<long>, 
	IRequest<DeletePersonExternDatabaseIdCommandResponse>
{
}

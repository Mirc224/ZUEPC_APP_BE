using MediatR;
using ZUEPC.Common.CQRS.Commands;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;

public class DeletePersonExternDatabaseIdCommandHandler :
	EPCDeleteSimpleModelBaseCommandHandler<PersonExternDatabaseIdModel>,
	IRequestHandler<DeletePersonExternDatabaseIdCommand, DeletePersonExternDatabaseIdCommandResponse>
{

	public DeletePersonExternDatabaseIdCommandHandler(IPersonExternDatabaseIdData repository)
	: base(repository) { }
	public async Task<DeletePersonExternDatabaseIdCommandResponse> Handle(DeletePersonExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		return await ProcessDeleteCommandAsync
			<DeletePersonExternDatabaseIdCommand, DeletePersonExternDatabaseIdCommandResponse>(request);
	}
}

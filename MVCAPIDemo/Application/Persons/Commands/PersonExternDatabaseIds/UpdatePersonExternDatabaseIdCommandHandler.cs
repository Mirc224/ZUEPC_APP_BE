using AutoMapper;
using MediatR;
using ZUEPC.Common.Commands;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;

public class UpdatePersonExternDatabaseIdCommandHandler :
	EPCUpdateSimpleModelCommandHandlerBase<PersonExternDatabaseIdModel>,
	IRequestHandler<UpdatePersonExternDatabaseIdCommand, UpdatePersonExternDatabaseIdCommandResponse>
{
	public UpdatePersonExternDatabaseIdCommandHandler(IMapper mapper, IPersonExternDatabaseIdData repository)
	: base(mapper, repository) { }

	public async Task<UpdatePersonExternDatabaseIdCommandResponse> Handle(UpdatePersonExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		return await ProcessUpdateCommandFromRequestAsync
			<UpdatePersonExternDatabaseIdCommand,
			UpdatePersonExternDatabaseIdCommandResponse>(request);
	}
}

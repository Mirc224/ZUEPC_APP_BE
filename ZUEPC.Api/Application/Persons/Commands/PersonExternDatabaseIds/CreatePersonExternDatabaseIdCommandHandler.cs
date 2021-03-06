using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;

public class CreatePersonExternDatabaseIdCommandHandler : 
	EPCCreateSimpleModelCommandHandlerBase<PersonExternDatabaseId, PersonExternDatabaseIdModel>,
	IRequestHandler<CreatePersonExternDatabaseIdCommand, CreatePersonExternDatabaseIdCommandResponse>
{

	public CreatePersonExternDatabaseIdCommandHandler(IMapper mapper, IPersonExternDatabaseIdData repository)
	: base(mapper, repository) { }

	public async Task<CreatePersonExternDatabaseIdCommandResponse> Handle(CreatePersonExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		return await ProcessInsertCommandAsync<CreatePersonExternDatabaseIdCommand, CreatePersonExternDatabaseIdCommandResponse>(request);
	}
}

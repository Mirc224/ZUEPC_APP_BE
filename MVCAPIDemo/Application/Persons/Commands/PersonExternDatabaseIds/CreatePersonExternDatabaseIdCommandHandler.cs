using AutoMapper;
using MediatR;
using ZUEPC.Common.Commands;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;

public class CreatePersonExternDatabaseIdCommandHandler : 
	CreateBaseHandler,
	IRequestHandler<CreatePersonExternDatabaseIdCommand, CreatePersonExternDatabaseIdCommandResponse>
{
	private readonly IPersonExternDatabaseIdData _repository;

	public CreatePersonExternDatabaseIdCommandHandler(IMapper mapper, IPersonExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreatePersonExternDatabaseIdCommandResponse> Handle(CreatePersonExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		PersonExternDatabaseIdModel insertModel = CreateInsertModelFromRequest
			<PersonExternDatabaseIdModel, CreatePersonExternDatabaseIdCommand>(request);
		long insertedId = await _repository.InsertModelAsync(insertModel);

		return CreateSuccessResponseWithDataFromInsertModel
			<CreatePersonExternDatabaseIdCommandResponse,
			PersonExternDatabaseId,
			PersonExternDatabaseIdModel>(insertModel, insertedId);
	}
}

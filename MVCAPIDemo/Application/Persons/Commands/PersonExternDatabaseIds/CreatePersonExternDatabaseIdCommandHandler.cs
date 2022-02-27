using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;

public class CreatePersonExternDatabaseIdCommandHandler : IRequestHandler<CreatePersonExternDatabaseIdCommand, CreatePersonExternDatabaseIdCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPersonExternDatabaseIdData _repository;

	public CreatePersonExternDatabaseIdCommandHandler(IMapper mapper, IPersonExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreatePersonExternDatabaseIdCommandResponse> Handle(CreatePersonExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		PersonExternDatabaseIdModel insertModel = _mapper.Map<PersonExternDatabaseIdModel>(request);
		long newId = await _repository.InsertPersonExternDatabaseIdAsync(insertModel);
		insertModel.Id = newId;
		PersonExternDatabaseId domain = _mapper.Map<PersonExternDatabaseId>(insertModel);
		return new() { Success = true, CreatedPersonExternDatabaseId = domain };
	}
}

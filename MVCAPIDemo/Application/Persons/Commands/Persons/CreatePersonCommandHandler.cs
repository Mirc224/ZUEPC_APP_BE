using AutoMapper;
using MediatR;
using ZUEPC.Common.Commands;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class CreatePersonCommandHandler : 
	CreateBaseHandler,
	IRequestHandler<CreatePersonCommand, CreatePersonCommandResponse>
{
	private readonly IPersonData _repository;

	public CreatePersonCommandHandler(IMapper mapper, IPersonData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreatePersonCommandResponse> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
	{
		PersonModel insertModel = CreateInsertModelFromRequest<PersonModel, CreatePersonCommand>(request);
		long insertedId = await _repository.InsertModelAsync(insertModel);
		return CreateSuccessResponseWithDataFromInsertModel
			<CreatePersonCommandResponse,
			Person,
			PersonModel>(insertModel, insertedId);
	}
}

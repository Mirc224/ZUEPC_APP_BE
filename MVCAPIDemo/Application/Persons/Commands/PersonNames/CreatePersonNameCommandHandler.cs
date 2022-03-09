using AutoMapper;
using MediatR;
using ZUEPC.Common.Commands;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Commands.PersonNames;

public class CreatePersonNameCommandHandler :
	CreateBaseHandler,
	IRequestHandler<CreatePersonNameCommand, CreatePersonNameCommandResponse>
{
	private readonly IPersonNameData _repository;

	public CreatePersonNameCommandHandler(IMapper mapper, IPersonNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreatePersonNameCommandResponse> Handle(CreatePersonNameCommand request, CancellationToken cancellationToken)
	{
		PersonNameModel insertModel = CreateInsertModelFromRequest
			<PersonNameModel, CreatePersonNameCommand>(request);
		long insertedId = await _repository.InsertModelAsync(insertModel);

		return CreateSuccessResponseWithDataFromInsertModel
			<CreatePersonNameCommandResponse,
			PersonName,
			PersonNameModel>(insertModel, insertedId);
	}
}

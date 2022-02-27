using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Commands.PersonNames;

public class CreatePersonNameCommandHandler : IRequestHandler<CreatePersonNameCommand, CreatePersonNameCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPersonNameData _repository;

	public CreatePersonNameCommandHandler(IMapper mapper, IPersonNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreatePersonNameCommandResponse> Handle(CreatePersonNameCommand request, CancellationToken cancellationToken)
	{
		PersonNameModel insertModel = _mapper.Map<PersonNameModel>(request);
		long createdRecordId = await _repository.InsertPersonNameAsync(insertModel);
		insertModel.Id = createdRecordId;
		PersonName domain = _mapper.Map<PersonName>(insertModel);
		return new() { Success = true, CreatedPersonName = domain };
	}
}

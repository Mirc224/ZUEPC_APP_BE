using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, CreatePersonCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPersonData _repository;

	public CreatePersonCommandHandler(IMapper mapper, IPersonData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreatePersonCommandResponse> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
	{
		PersonModel insertModel = _mapper.Map<PersonModel>(request);
		insertModel.CreatedAt = DateTime.UtcNow;
		if (request.VersionDate is null)
		{
			insertModel.CreatedAt = DateTime.UtcNow;
		}
		long personId = await _repository.InsertPersonAsync(insertModel);
		Person mappedModel = _mapper.Map<Person>(insertModel);
		mappedModel.Id = personId;
		return new() { Success = true, Person = mappedModel };
	}
}

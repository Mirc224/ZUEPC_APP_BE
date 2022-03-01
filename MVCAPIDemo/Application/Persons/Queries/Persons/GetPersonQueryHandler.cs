using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.Persons;

public class GetPersonQueryHandler : IRequestHandler<GetPersonQuery, GetPersonQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPersonData _repository;

	public GetPersonQueryHandler(IMapper mapper, IPersonData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetPersonQueryResponse> Handle(GetPersonQuery request, CancellationToken cancellationToken)
	{
		PersonModel? personModel = await _repository.GetPersonByIdAsync(request.PersonId);
		if (personModel is null)
		{
			return new() { Success = false };
		}
		Person mappedPerson = _mapper.Map<Person>(request);
		return new() { Success = true, Person = mappedPerson };
	}
}

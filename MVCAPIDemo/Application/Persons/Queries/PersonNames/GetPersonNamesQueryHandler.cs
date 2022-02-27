using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.PersonNames;

public class GetPersonNamesQueryHandler : IRequestHandler<GetPersonNamesQuery, GetPersonNamesQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPersonNameData _repository;

	public GetPersonNamesQueryHandler(IMapper mapper, IPersonNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetPersonNamesQueryResponse> Handle(GetPersonNamesQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PersonNameModel> queryResult = await _repository.GetPersonNamesByPersonIdAsync(request.PersonId);
		List<PersonName> mappedResult = _mapper.Map<List<PersonName>>(queryResult);

		return new() { Success = true, PersonNames = mappedResult };
	}
}

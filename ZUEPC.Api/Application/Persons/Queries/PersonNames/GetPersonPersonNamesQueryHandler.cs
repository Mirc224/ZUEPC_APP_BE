using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.PersonNames;

public class GetPersonPersonNamesQueryHandler : IRequestHandler<GetPersonPersonNamesQuery, GetPersonPersonNamesQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPersonNameData _repository;

	public GetPersonPersonNamesQueryHandler(IMapper mapper, IPersonNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetPersonPersonNamesQueryResponse> Handle(GetPersonPersonNamesQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PersonNameModel> queryResult = await _repository.GetPersonNamesByPersonIdAsync(request.PersonId);
		List<PersonName> mappedResult = _mapper.Map<List<PersonName>>(queryResult);

		return new() { Success = true, Data = mappedResult };
	}
}

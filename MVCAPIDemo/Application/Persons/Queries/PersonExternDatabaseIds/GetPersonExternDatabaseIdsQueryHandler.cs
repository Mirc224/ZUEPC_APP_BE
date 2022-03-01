using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;

public class GetPersonExternDatabaseIdsQueryHandler : IRequestHandler<GetPersonExternDatabaseIdsQuery, GetPersonExternDatabaseIdsQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPersonExternDatabaseIdData _repository;

	public GetPersonExternDatabaseIdsQueryHandler(IMapper mapper, IPersonExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetPersonExternDatabaseIdsQueryResponse> Handle(GetPersonExternDatabaseIdsQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PersonExternDatabaseIdModel> queryResult = await _repository.GetPersonExternDatabaseIdsByPersonIdAsync(request.PersonId);
		if (queryResult is null)
		{
			return new() { Success = false };
		}
		List<PersonExternDatabaseId> mappedResult = _mapper.Map<List<PersonExternDatabaseId>>(queryResult);

		return new() { PersonExternDatabaseIds = mappedResult };
	}
}

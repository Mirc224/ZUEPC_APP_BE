using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;

public class GetPersonPersonExternDatabaseIdsQueryHandler : IRequestHandler<GetPersonPersonExternDatabaseIdsQuery, GetPersonPersonExternDatabaseIdsQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPersonExternDatabaseIdData _repository;

	public GetPersonPersonExternDatabaseIdsQueryHandler(IMapper mapper, IPersonExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetPersonPersonExternDatabaseIdsQueryResponse> Handle(GetPersonPersonExternDatabaseIdsQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PersonExternDatabaseIdModel> queryResult = await _repository.GetPersonExternDatabaseIdsByPersonIdAsync(request.PersonId);
		if (queryResult is null)
		{
			return new() { Success = false };
		}
		List<PersonExternDatabaseId> mappedResult = _mapper.Map<List<PersonExternDatabaseId>>(queryResult);

		return new() { Data = mappedResult };
	}
}

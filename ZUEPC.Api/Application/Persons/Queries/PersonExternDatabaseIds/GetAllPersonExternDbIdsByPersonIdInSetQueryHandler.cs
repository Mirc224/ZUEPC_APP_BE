using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Api.Application.Persons.Queries.PersonExternDatabaseIds;

public class GetAllPersonExternDbIdsByPersonIdInSetQueryHandler :
	IRequestHandler<GetAllPersonExternDbIdsByPersonIdInSetQuery, GetAllPersonExternDbIdsByPersonIdInSetQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPersonExternDatabaseIdData _repository;

	public GetAllPersonExternDbIdsByPersonIdInSetQueryHandler(IMapper mapper, IPersonExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetAllPersonExternDbIdsByPersonIdInSetQueryResponse> Handle(GetAllPersonExternDbIdsByPersonIdInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PersonExternDatabaseIdModel> externIds = await _repository.GetAllPersonExternDbIdsByPersonIdInSetAsync(request.PersonIds);
		if (externIds is null)
		{
			return new() { Success = false };
		}

		List<PersonExternDatabaseId> mapedResult = _mapper.Map<List<PersonExternDatabaseId>>(externIds);
		return new() { Success = true, Data = mapedResult };
	}
}

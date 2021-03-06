using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;

public class GetAllPersonsExternDbIdsInSetQueryHandler :
	IRequestHandler<GetAllPersonsExternDbIdsInSetQuery, GetAllPersonsExternDbIdsInSetQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPersonExternDatabaseIdData _repository;

	public GetAllPersonsExternDbIdsInSetQueryHandler(IMapper mapper, IPersonExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetAllPersonsExternDbIdsInSetQueryResponse> Handle(GetAllPersonsExternDbIdsInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PersonExternDatabaseIdModel> externIds = await _repository.GetAllPersonExternDbIdsByIdentifierValueSetAsync(request.SearchedExternIdentifiers);
		if (externIds is null)
		{
			return new() { Success = false };
		}

		List<PersonExternDatabaseId> mapedResult = _mapper.Map<List<PersonExternDatabaseId>>(externIds);
		return new() { Success = true, Data = mapedResult };
	}
}

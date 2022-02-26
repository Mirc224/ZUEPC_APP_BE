using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

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
		var externIds = await _repository.GetAllPersonExternDbIdsByIdentifierValueSetAsync(request.SearchedExternIdentifiers);
		if (externIds is null)
		{
			return new() { Success = false };
		}

		var mapedResult = _mapper.Map<List<PersonExternDatabaseId>>(externIds);
		return new() { Success = true, ExternDatabaseIds = mapedResult };
	}
}

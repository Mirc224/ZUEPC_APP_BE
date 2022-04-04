using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;

public class GetAllInstitutionExternDbIdsInSetQueryHandler : IRequestHandler<GetAllInstitutionExternDbIdsInSetQuery, GetAllInstitutionExternDbIdsInSetQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IInstitutionExternDatabaseIdData _repository;

	public GetAllInstitutionExternDbIdsInSetQueryHandler(IMapper mapper, IInstitutionExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	
	public async Task<GetAllInstitutionExternDbIdsInSetQueryResponse> Handle(GetAllInstitutionExternDbIdsInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<InstitutionExternDatabaseIdModel> externIds = await _repository.GetAllInstitutionExternDbIdsByIdentifierValueSetAsync(request.SearchedExternIdentifiers);
		if (externIds is null)
		{
			return new() { Success = false };
		}

		List<InstitutionExternDatabaseId> mapedResult = _mapper.Map<List<InstitutionExternDatabaseId>>(externIds);
		return new() { Success = true, Data = mapedResult };
	}
}

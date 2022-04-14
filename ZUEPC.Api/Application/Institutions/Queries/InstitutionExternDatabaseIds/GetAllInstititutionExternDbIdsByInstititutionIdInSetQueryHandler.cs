using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Api.Application.Institutions.Queries.InstitutionExternDatabaseIds;

public class GetAllInstititutionExternDbIdsByInstititutionIdInSetQueryHandler :
	IRequestHandler<GetAllInstititutionExternDbIdsByInstititutionIdInSetQuery, GetAllInstititutionExternDbIdsByInstititutionIdInSetQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IInstitutionExternDatabaseIdData _repository;

	public GetAllInstititutionExternDbIdsByInstititutionIdInSetQueryHandler(IMapper mapper, IInstitutionExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetAllInstititutionExternDbIdsByInstititutionIdInSetQueryResponse> Handle(GetAllInstititutionExternDbIdsByInstititutionIdInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<InstitutionExternDatabaseIdModel> externDatabaseIdsModel = await _repository.GetAllInstitutionExternDbIdsByInstitutionIdInSetAsync(request.InstitutionIds);
		if (externDatabaseIdsModel is null)
		{
			return new() { Success = false };
		}

		IEnumerable<InstitutionExternDatabaseId> mapedResult = _mapper.Map<List<InstitutionExternDatabaseId>>(externDatabaseIdsModel);
		return new() { Success = true, Data = mapedResult };
	}
}

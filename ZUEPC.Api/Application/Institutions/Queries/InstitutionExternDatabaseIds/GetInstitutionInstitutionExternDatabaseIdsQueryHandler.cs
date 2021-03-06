using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;

public class GetInstitutionInstitutionExternDatabaseIdsQueryHandler : IRequestHandler<GetInstitutionInstitutionExternDatabaseIdsQuery, GetInstitutionInstitutionExternDatabaseIdsQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IInstitutionExternDatabaseIdData _repository;

	public GetInstitutionInstitutionExternDatabaseIdsQueryHandler(IMapper mapper, IInstitutionExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetInstitutionInstitutionExternDatabaseIdsQueryResponse> Handle(GetInstitutionInstitutionExternDatabaseIdsQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<InstitutionExternDatabaseIdModel> queryResult = await _repository
			.GetInstitutionExternDatabaseIdsByInstitutionIdAsync(request.InstitutionId);

		List<InstitutionExternDatabaseId> mappedResult = _mapper.Map<List<InstitutionExternDatabaseId>>(queryResult);

		return new() { Success = true, Data = mappedResult };
	}
}

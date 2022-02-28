using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;

public class GetInstitutionExternDatabaseIdsQueryHandler : IRequestHandler<GetInstitutionExternDatabaseIdsQuery, GetInstitutionExternDatabaseIdsQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IInstitutionExternDatabaseIdData _repository;

	public GetInstitutionExternDatabaseIdsQueryHandler(IMapper mapper, IInstitutionExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetInstitutionExternDatabaseIdsQueryResponse> Handle(GetInstitutionExternDatabaseIdsQuery request, CancellationToken cancellationToken)
	{
		Task<IEnumerable<InstitutionExternDatabaseIdModel>> queryResult = _repository
			.GetInstitutionExternDatabaseIdsByInstitutionIdAsync(request.InstitutionId);

		List<InstitutionExternDatabaseId> mappedResult = _mapper.Map<List<InstitutionExternDatabaseId>>(queryResult);

		return new() { Success = true, InstitutionExternDatabaseIds = mappedResult };
	}
}

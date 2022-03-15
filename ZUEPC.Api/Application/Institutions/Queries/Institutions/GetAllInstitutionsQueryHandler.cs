using AutoMapper;
using MediatR;
using ZUEPC.Api.Common.CQRS.QueryHandlers;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.Institutions;

public class GetAllInstitutionsQueryHandler :
	GetModelsWithFiltersPagedQueryHandlerBase<IInstitutionData, Institution, InstitutionModel, InstitutionFilter>,
	IRequestHandler<GetAllInstitutionsQuery, GetAllInstitutionsQueryResponse>
{
	public GetAllInstitutionsQueryHandler(IMapper mapper, IInstitutionData repository)
	: base(mapper, repository) { }

	public async Task<GetAllInstitutionsQueryResponse> Handle(GetAllInstitutionsQuery request, CancellationToken cancellationToken)
	{
		if(request.QueryFilter is null)
		{
			return await ProcessQueryAsync<GetAllInstitutionsQuery, GetAllInstitutionsQueryResponse>(request);
		}
		return await ProcessQueryWithFilterAsync<GetAllInstitutionsQuery, GetAllInstitutionsQueryResponse>(request);
	}
}

using AutoMapper;
using MediatR;
using ZUEPC.Api.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Api.Application.Institutions.Queries.InstitutionNames;

public class GetAllInstitutionNamesQueryHandler :
	GetModelsWithFiltersPagedQueryHandlerBase<IInstitutionNameData, InstitutionName, InstitutionNameModel, InstitutionNameFilter>,
	IRequestHandler<GetAllInstitutionNamesQuery, GetAllInstitutionNamesQueryResponse>
{
	public GetAllInstitutionNamesQueryHandler(IMapper mapper, IInstitutionNameData repository) : base(mapper, repository)
	{
	}

	public async Task<GetAllInstitutionNamesQueryResponse> Handle(GetAllInstitutionNamesQuery request, CancellationToken cancellationToken)
	{
		if (request.QueryFilter is null)
		{
			return await ProcessQueryAsync<GetAllInstitutionNamesQuery, GetAllInstitutionNamesQueryResponse>(request);
		}
		return await ProcessQueryWithFilterAsync<GetAllInstitutionNamesQuery, GetAllInstitutionNamesQueryResponse>(request);
	}
}


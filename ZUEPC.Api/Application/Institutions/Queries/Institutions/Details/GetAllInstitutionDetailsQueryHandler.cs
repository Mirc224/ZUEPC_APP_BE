using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Institutions.Queries.InstitutionExternDatabaseIds;
using ZUEPC.Api.Application.Institutions.Queries.InstitutionNames;
using ZUEPC.Application.Institutions.Entities.Details;
using ZUEPC.Application.Institutions.Queries.Institutions.Details.BaseHandlers;
using ZUEPC.Base.Extensions;
using ZUEPC.Base.Helpers;
using ZUEPC.Base.QueryFilters;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details;

public class GetAllInstitutionDetailsQueryHandler :
	EPCInstitutionDetailsQueryHandlerBase,
	IRequestHandler<GetAllInstitutionDetailsQuery, GetAllInstitutionDetailsQueryResponse>
{
	public GetAllInstitutionDetailsQueryHandler(IMapper mapper, IMediator mediator)
		: base(mapper, mediator) { }
	public async Task<GetAllInstitutionDetailsQueryResponse> Handle(GetAllInstitutionDetailsQuery request, CancellationToken cancellationToken)
	{
		GetAllInstitutionsQueryResponse response = await _mediator.Send(new GetAllInstitutionsQuery()
		{
			PaginationFilter = request.PaginationFilter,
			UriService = request.UriService,
			Route = request.Route,
			QueryFilter = request.QueryFilter
		});

		if (!response.Success || response.Data is null)
		{
			return new() { Success = false };
		}
		int totalRecords = response.TotalRecords;

		if (!response.Data.Any())
		{
			return PaginationHelper.ProcessResponse<GetAllInstitutionDetailsQueryResponse, InstitutionDetails, InstitutionFilter>(
			new List<InstitutionDetails>(),
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
		}

		IEnumerable<InstitutionDetails> result = _mapper.Map<List<InstitutionDetails>>(response.Data);
		IEnumerable<long> personIds = result.Select(x => x.Id).ToHashSet();

		IEnumerable<InstitutionName> allNamesByInstitutionIds = await GetInstitutionNamesWithInstitutionIdInSet(personIds);
		IEnumerable<InstitutionExternDatabaseId> allExternDbIdsByInstitutionIds = await GetInstitutionExternDbIdsWithInstitutionIdInSet(personIds);

		IEnumerable<IGrouping<long, InstitutionName>> personNamesGroupByPersonId = allNamesByInstitutionIds.GroupBy(x => x.InstitutionId);
		IEnumerable<IGrouping<long, InstitutionExternDatabaseId>> personExternDbIdsGroupByPersonId = allExternDbIdsByInstitutionIds.GroupBy(x => x.InstitutionId);

		foreach (InstitutionDetails institution in result)
		{
			institution.Names = personNamesGroupByPersonId.Where(x => x.Key == institution.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			institution.ExternDatabaseIds = personExternDbIdsGroupByPersonId.Where(x => x.Key == institution.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
		}

		return PaginationHelper.ProcessResponse<GetAllInstitutionDetailsQueryResponse, InstitutionDetails, InstitutionFilter>(
			result,
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
	}

	private async Task<IEnumerable<InstitutionName>> GetInstitutionNamesWithInstitutionIdInSet(IEnumerable<long> institutionIds)
	{
		return (await _mediator.Send(new GetAllInstititutionNamesByInstititutionIdInSetQuery() { InstitutionIds = institutionIds }))
		.Data
		.OrEmptyIfNull();
	}

	private async Task<IEnumerable<InstitutionExternDatabaseId>> GetInstitutionExternDbIdsWithInstitutionIdInSet(IEnumerable<long> institutionIds)
	{
		return (await _mediator.Send(new GetAllInstititutionExternDbIdsByInstititutionIdInSetQuery() { InstitutionIds = institutionIds }))
		.Data
		.OrEmptyIfNull();
	}

}

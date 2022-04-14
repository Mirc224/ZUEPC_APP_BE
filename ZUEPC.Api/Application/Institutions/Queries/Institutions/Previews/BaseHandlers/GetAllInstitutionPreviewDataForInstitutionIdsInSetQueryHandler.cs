using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Institutions.Queries.InstitutionExternDatabaseIds;
using ZUEPC.Api.Application.Institutions.Queries.InstitutionNames;
using ZUEPC.Base.Extensions;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Api.Application.Institutions.Queries.Institutions.Previews.BaseHandlers;

public class GetAllInstitutionPreviewDataForInstitutionIdsInSetQueryHandler :
	IRequestHandler<GetAllInstitutionPreviewDataForInstitutionIdsInSetQuery, GetAllInstitutionPreviewDataForInstitutionIdsInSetQueryResponse>
{
	private readonly IMediator _mediator;

	public GetAllInstitutionPreviewDataForInstitutionIdsInSetQueryHandler(IMediator mediator)
	{
		_mediator = mediator;
	}
	
	public async Task<GetAllInstitutionPreviewDataForInstitutionIdsInSetQueryResponse> Handle(GetAllInstitutionPreviewDataForInstitutionIdsInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<long> institutionIds = request.InstitutionIds.ToHashSet();

		IEnumerable<InstitutionName> allNamesByInstitutionIds = await GetInstitutionNamesWithInstitutionIdInSet(institutionIds);
		IEnumerable<InstitutionExternDatabaseId> allExternDbIdsByInstitutionIds = await GetInstitutionExternDbIdsWithInstitutionIdInSet(institutionIds);

		return new() { Success = true, InstitutionNames = allNamesByInstitutionIds, InstitutionExternDatabaseIds = allExternDbIdsByInstitutionIds };
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

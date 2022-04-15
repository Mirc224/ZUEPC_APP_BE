using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Institutions.Queries.Institutions.Previews.BaseHandlers;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Base.Extensions;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Previews.BaseHandlers;

public abstract class EPCInstitutionPreviewQueryHandlerBase
{
	protected readonly IMapper _mapper;
	protected readonly IMediator _mediator;

	public EPCInstitutionPreviewQueryHandlerBase(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	protected async Task<InstitutionPreview> ProcessInstitutionPreview(Institution institutionDomain)
	{
		return (await ProcessInstitutionPreviews(new Institution[] {institutionDomain})).First();
	}

	protected async Task<IEnumerable<InstitutionPreview>> ProcessInstitutionPreviews(IEnumerable<Institution> institutionDomains)
	{
		IEnumerable<InstitutionPreview> result = _mapper.Map<List<InstitutionPreview>>(institutionDomains);
		IEnumerable<long> institutionIds = result.Select(x => x.Id);

		GetAllInstitutionPreviewDataForInstitutionIdsInSetQueryResponse previewData = await _mediator
			.Send(new GetAllInstitutionPreviewDataForInstitutionIdsInSetQuery() { InstitutionIds = institutionIds });

		IEnumerable<IGrouping<long, InstitutionName>> namesGroupByInstitutionId = previewData
			.InstitutionNames
			.GroupBy(x => x.InstitutionId);
		IEnumerable<IGrouping<long, InstitutionExternDatabaseId>> externDbIdsGroupByInstitutionId = previewData
			.InstitutionExternDatabaseIds
			.GroupBy(x => x.InstitutionId);

		foreach (InstitutionPreview institution in result)
		{
			institution.Names = namesGroupByInstitutionId.Where(x => x.Key == institution.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			institution.ExternDatabaseIds = externDbIdsGroupByInstitutionId.Where(x => x.Key == institution.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
		}
		return result;
	}
}

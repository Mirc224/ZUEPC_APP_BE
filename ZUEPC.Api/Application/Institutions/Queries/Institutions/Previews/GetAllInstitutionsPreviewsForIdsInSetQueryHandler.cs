using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Institutions.Queries.Institutions.Previews.BaseHandlers;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Base.Extensions;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Api.Application.Institutions.Queries.Institutions.Previews;

public class GetAllInstitutionsPreviewsForIdsInSetQueryHandler :
	IRequestHandler<GetAllInstitutionsPreviewsForIdsInSetQuery, GetAllInstitutionsPreviewsForIdsInSetQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public GetAllInstitutionsPreviewsForIdsInSetQueryHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	public async Task<GetAllInstitutionsPreviewsForIdsInSetQueryResponse> Handle(GetAllInstitutionsPreviewsForIdsInSetQuery request, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(new GetAllInstitutionsWithIdInSetQuery()
		{
			InstitutionIds= request.InstitutionIds,
		});

		if (!response.Success)
		{
			return new() { Success = false };
		}

		IEnumerable<InstitutionPreview> result = _mapper.Map<List<InstitutionPreview>>(response.Data);
		IEnumerable<long> institutionIds = result.Select(x => x.Id);
		GetAllInstitutionPreviewDataForInstitutionIdsInSetQueryResponse previewData = await _mediator
			.Send(new GetAllInstitutionPreviewDataForInstitutionIdsInSetQuery() { InstitutionIds = institutionIds });

		IEnumerable<IGrouping<long, InstitutionName>> personNamesGroupByPersonId = previewData.InstitutionNames.GroupBy(x => x.InstitutionId);
		IEnumerable<IGrouping<long, InstitutionExternDatabaseId>> personExternDbIdsGroupByPersonId = previewData.InstitutionExternDatabaseIds.GroupBy(x => x.InstitutionId);

		foreach (InstitutionPreview institution in result)
		{
			institution.Names = personNamesGroupByPersonId.Where(x => x.Key == institution.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			institution.ExternDatabaseIds = personExternDbIdsGroupByPersonId.Where(x => x.Key == institution.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
		}

		return new() { Success = true, Data = result };
	}
}

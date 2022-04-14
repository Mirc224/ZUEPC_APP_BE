using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Persons.Queries.Persons.Previews.BaseHandlers;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Base.Extensions;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Api.Application.Persons.Queries.Persons.Previews;

public class GetAllPersonPreviewsForIdsInSetQueryHandler :
	IRequestHandler<GetAllPersonPreviewsForIdsInSetQuery, GetAllPersonPreviewsForIdsInSetQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public GetAllPersonPreviewsForIdsInSetQueryHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	public async Task<GetAllPersonPreviewsForIdsInSetQueryResponse> Handle(GetAllPersonPreviewsForIdsInSetQuery request, CancellationToken cancellationToken)
	{
		GetAllPersonsWithIdInSetQueryResponse response = await _mediator.Send(new GetAllPersonsWithIdInSetQuery()
		{
			PersonIds = request.PersonIds,
		});
		
		if(!response.Success)
		{
			return new() { Success = false };
		}

		IEnumerable<PersonPreview> result = _mapper.Map<List<PersonPreview>>(response.Data);
		IEnumerable<long> personIds = result.Select(x => x.Id);
		GetAllPersonPreviewDataForPersonIdsInSetQueryResponse previewData = await _mediator
			.Send(new GetAllPersonPreviewDataForPersonIdsInSetQuery() { PersonIds = personIds });

		IEnumerable<IGrouping<long, PersonName>> personNamesGroupByPersonId = previewData.PersonNames.GroupBy(x => x.PersonId);
		IEnumerable<IGrouping<long, PersonExternDatabaseId>> personExternDbIdsGroupByPersonId = previewData.PersonExternDatabaseIds.GroupBy(x => x.PersonId);

		foreach (PersonPreview person in result)
		{
			person.Names = personNamesGroupByPersonId.Where(x => x.Key == person.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			person.ExternDatabaseIds = personExternDbIdsGroupByPersonId.Where(x => x.Key == person.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
		}

		return new() { Success = true, Data = result };
	}
}

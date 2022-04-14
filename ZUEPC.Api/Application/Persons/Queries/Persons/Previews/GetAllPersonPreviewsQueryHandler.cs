using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Persons.Queries.PersonExternDatabaseIds;
using ZUEPC.Api.Application.Persons.Queries.PersonNames;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.Persons.Queries.Persons.Previews.BaseHandlers;
using ZUEPC.Base.Extensions;
using ZUEPC.Base.Helpers;
using ZUEPC.Base.QueryFilters;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews;

public class GetAllPersonPreviewsQueryHandler :
	IRequestHandler<GetAllPersonPreviewsQuery, GetAllPersonPreviewsQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public GetAllPersonPreviewsQueryHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}
	
	public async Task<GetAllPersonPreviewsQueryResponse> Handle(GetAllPersonPreviewsQuery request, CancellationToken cancellationToken)
	{
		GetAllPersonsQueryResponse response = await _mediator.Send(new GetAllPersonsQuery() { 
			PaginationFilter = request.PaginationFilter,
			UriService = request.UriService,
			Route = request.Route,
			QueryFilter = request.QueryFilter
		});
		
		if(!response.Success || response.Data is null) 
		{
			return new() { Success = false };
		}
		int totalRecords = response.TotalRecords;

		if (!response.Data.Any())
		{
			return PaginationHelper.ProcessResponse<GetAllPersonPreviewsQueryResponse, PersonPreview, PersonFilter>(
			new List<PersonPreview>(),
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
		}

		IEnumerable<PersonPreview> result = _mapper.Map<List<PersonPreview>>(response.Data);
		IEnumerable<long> personIds = result.Select(x => x.Id).ToHashSet();

		IEnumerable<PersonName> allPersonNamesByPersonIds = await GetPersonNamesWithPersonIdInSet(personIds);
		IEnumerable<PersonExternDatabaseId> allPersonExternDbIdsByPersonIds = await GetPersonExternDbIdsWithPersonIdInSet(personIds);

		IEnumerable<IGrouping<long, PersonName>> personNamesGroupByPersonId = allPersonNamesByPersonIds.GroupBy(x => x.PersonId);
		IEnumerable<IGrouping<long, PersonExternDatabaseId>> personExternDbIdsGroupByPersonId = allPersonExternDbIdsByPersonIds.GroupBy(x => x.PersonId);

		foreach (PersonPreview person in result)
		{
			person.Names = personNamesGroupByPersonId.Where(x => x.Key == person.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			person.ExternDatabaseIds = personExternDbIdsGroupByPersonId.Where(x => x.Key == person.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
		}

		return PaginationHelper.ProcessResponse<GetAllPersonPreviewsQueryResponse, PersonPreview, PersonFilter>(
			result,
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
	}

	private async Task<IEnumerable<PersonName>> GetPersonNamesWithPersonIdInSet(IEnumerable<long> personIds)
	{
		IEnumerable<PersonName> result = (await _mediator.Send(new GetAllPersonNamesByPersonIdInSetQuery() { PersonIds = personIds }))
			.Data
			.OrEmptyIfNull();
		return result;
	}

	private async Task<IEnumerable<PersonExternDatabaseId>> GetPersonExternDbIdsWithPersonIdInSet(IEnumerable<long> personIds)
	{
		IEnumerable<PersonExternDatabaseId> result = (await _mediator.Send(new GetAllPersonExternDbIdsByPersonIdInSetQuery() { PersonIds = personIds }))
			.Data
			.OrEmptyIfNull();
		return result;
	}
}

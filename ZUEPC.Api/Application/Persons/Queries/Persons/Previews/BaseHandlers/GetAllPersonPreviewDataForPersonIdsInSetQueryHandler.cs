using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Persons.Queries.PersonExternDatabaseIds;
using ZUEPC.Api.Application.Persons.Queries.PersonNames;
using ZUEPC.Base.Extensions;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Api.Application.Persons.Queries.Persons.Previews.BaseHandlers;

public class GetAllPersonPreviewDataForPersonIdsInSetQueryHandler :
	IRequestHandler<GetAllPersonPreviewDataForPersonIdsInSetQuery, GetAllPersonPreviewDataForPersonIdsInSetQueryResponse>
{
	private readonly IMediator _mediator;

	public GetAllPersonPreviewDataForPersonIdsInSetQueryHandler(IMediator mediator)
	{
		_mediator = mediator;
	}
	public async Task<GetAllPersonPreviewDataForPersonIdsInSetQueryResponse> Handle(GetAllPersonPreviewDataForPersonIdsInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<long> personIds = request.PersonIds.ToHashSet();
		IEnumerable<PersonName> allPersonNamesByPersonIds = await GetPersonNamesWithPersonIdInSet(personIds);
		IEnumerable<PersonExternDatabaseId> allPersonExternDbIdsByPersonIds = await GetPersonExternDbIdsWithPersonIdInSet(personIds);
		return new() { Success = true, PersonNames = allPersonNamesByPersonIds, PersonExternDatabaseIds=allPersonExternDbIdsByPersonIds };
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

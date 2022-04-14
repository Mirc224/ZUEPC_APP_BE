﻿using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Persons.Queries.Persons.Previews.BaseHandlers;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Queries.PersonNames;
using ZUEPC.Base.Extensions;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews.BaseHandlers;

public abstract class EPCPersonPreviewQueryHandlerBase
{
	protected readonly IMapper _mapper;
	protected readonly IMediator _mediator;

	public EPCPersonPreviewQueryHandlerBase(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	protected async Task<PersonPreview> ProcessPersonPreview(Person personDomain)
	{
		long personId = personDomain.Id;
		PersonPreview resultPreview = _mapper.Map<PersonPreview>(personDomain);
		resultPreview.Names = (await _mediator.Send(new GetPersonPersonNamesQuery()
		{
			PersonId = personId
		})).Data;
		resultPreview.ExternDatabaseIds = (await _mediator.Send(new GetPersonPersonExternDatabaseIdsQuery()
		{
			PersonId = personId
		})).Data;
		return resultPreview;
	}

	protected async Task<IEnumerable<PersonPreview>> ProcessPersonPreviews(IEnumerable<Person> personDomains)
	{
		IEnumerable<PersonPreview> result = _mapper.Map<List<PersonPreview>>(personDomains);
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

		return result;
	}
}

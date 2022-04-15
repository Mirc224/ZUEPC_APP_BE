using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Persons.Queries.PersonExternDatabaseIds;
using ZUEPC.Api.Application.Persons.Queries.PersonNames;
using ZUEPC.Application.Persons.Entities.Details;
using ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Queries.PersonNames;
using ZUEPC.Base.Extensions;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.Persons.Details.BaseHandler;

public abstract class EPCPersonDetailsQueryHandlerBase
{
	protected readonly IMapper _mapper;
	protected readonly IMediator _mediator;

	public EPCPersonDetailsQueryHandlerBase(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	protected async Task<PersonDetails> ProcessPersonDetails(Person personDomain)
	{
		return (await ProcessPersonDetails(new Person[] {personDomain})).First();
	}

	protected async Task<IEnumerable<PersonDetails>> ProcessPersonDetails(IEnumerable<Person> personDomains)
	{
		IEnumerable<PersonDetails> result = _mapper.Map<List<PersonDetails>>(personDomains);
		IEnumerable<long> personIds = result.Select(x => x.Id).ToHashSet();
		IEnumerable<PersonName> allPersonNamesByPersonIds = await GetPersonNamesWithPersonIdInSet(personIds);
		IEnumerable<PersonExternDatabaseId> allPersonExternDbIdsByPersonIds = await GetPersonExternDbIdsWithPersonIdInSet(personIds);

		IEnumerable<IGrouping<long, PersonName>> personNamesGroupByPersonId = allPersonNamesByPersonIds.GroupBy(x => x.PersonId);
		IEnumerable<IGrouping<long, PersonExternDatabaseId>> personExternDbIdsGroupByPersonId = allPersonExternDbIdsByPersonIds.GroupBy(x => x.PersonId);

		foreach (PersonDetails person in result)
		{
			person.Names = personNamesGroupByPersonId.Where(x => x.Key == person.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			person.ExternDatabaseIds = personExternDbIdsGroupByPersonId.Where(x => x.Key == person.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
		}

		return result;
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

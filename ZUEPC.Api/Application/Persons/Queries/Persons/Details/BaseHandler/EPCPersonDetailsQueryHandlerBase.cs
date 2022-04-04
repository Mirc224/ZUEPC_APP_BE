using AutoMapper;
using MediatR;
using ZUEPC.Application.Persons.Entities.Details;
using ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Queries.PersonNames;
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
		long personId = personDomain.Id;
		PersonDetails result = _mapper.Map<PersonDetails>(personDomain);
		result.Names = (await _mediator.Send(new GetPersonPersonNamesQuery()
		{
			PersonId = personId
		})).Data;

		result.ExternDatabaseIds = (await _mediator.Send(new GetPersonPersonExternDatabaseIdsQuery()
		{
			PersonId = personId
		})).Data;

		return result;
	}
}

using AutoMapper;
using MediatR;
using ZUEPC.Application.Persons.Entities.Details;
using ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Queries.PersonNames;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.Persons.Details;

public class GetPersonDetailsQueryHandler : IRequestHandler<GetPersonDetailsQuery, GetPersonDetailsQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public GetPersonDetailsQueryHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}
	
	public async Task<GetPersonDetailsQueryResponse> Handle(GetPersonDetailsQuery request, CancellationToken cancellationToken)
	{
		long personId = request.PersonId;
		Person? person = (await _mediator.Send(new GetPersonQuery()
		{
			PersonId = personId
		})).Person;

		if (person is null)
		{
			return new() { Success = false };
		}
		PersonDetails result = _mapper.Map<PersonDetails>(person);
		result.Names = (await _mediator.Send(new GetPersonNamesQuery()
		{
			PersonId = personId
		})).PersonNames;

		result.ExternDatabaseIds = (await _mediator.Send(new GetPersonExternDatabaseIdsQuery()
		{
			PersonId = personId
		})).PersonExternDatabaseIds;


		return new() { Success = true, PersonDetails = result };
	}
}

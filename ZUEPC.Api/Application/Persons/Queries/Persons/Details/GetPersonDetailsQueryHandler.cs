using AutoMapper;
using MediatR;
using ZUEPC.Application.Persons.Entities.Details;
using ZUEPC.Application.Persons.Queries.Persons.Details.BaseHandler;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.Persons.Details;

public class GetPersonDetailsQueryHandler :
	EPCPersonDetailsQueryHandlerBase,
	IRequestHandler<GetPersonDetailsQuery, GetPersonDetailsQueryResponse>
{
	public GetPersonDetailsQueryHandler(IMapper mapper, IMediator mediator)
	: base(mapper, mediator) { }
	
	public async Task<GetPersonDetailsQueryResponse> Handle(GetPersonDetailsQuery request, CancellationToken cancellationToken)
	{
		long personId = request.Id;
		Person? person = (await _mediator.Send(new GetPersonQuery()
		{
			Id = personId
		})).Data;

		if (person is null)
		{
			return new() { Success = false };
		}
		PersonDetails result = await ProcessPersonDetails(person);

		return new() { Success = true, Data = result };
	}
}

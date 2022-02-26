using MediatR;

namespace ZUEPC.Application.Persons.Queries.Persons;

public class GetPersonQuery : IRequest<GetPersonQueryResponse>
{
	public long PersonId { get; set; }
}

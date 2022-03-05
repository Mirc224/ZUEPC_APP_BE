using MediatR;

namespace ZUEPC.Application.Persons.Queries.Persons.Details;

public class GetPersonDetailsQuery : IRequest<GetPersonDetailsQueryResponse>
{
	public long PersonId { get; set; }
}

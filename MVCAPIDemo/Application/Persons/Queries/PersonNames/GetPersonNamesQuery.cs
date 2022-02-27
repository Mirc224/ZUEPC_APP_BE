using MediatR;

namespace ZUEPC.Application.Persons.Queries.PersonNames;

public class GetPersonNamesQuery : IRequest<GetPersonNamesQueryResponse>
{
	public long PersonId { get; set; }
}

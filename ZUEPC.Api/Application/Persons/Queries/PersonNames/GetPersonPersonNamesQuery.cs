using MediatR;

namespace ZUEPC.Application.Persons.Queries.PersonNames;

public class GetPersonPersonNamesQuery : IRequest<GetPersonPersonNamesQueryResponse>
{
	public long PersonId { get; set; }
}

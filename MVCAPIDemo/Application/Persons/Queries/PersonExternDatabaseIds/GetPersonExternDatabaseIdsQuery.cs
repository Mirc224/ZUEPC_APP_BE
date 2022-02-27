using MediatR;

namespace ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;

public class GetPersonExternDatabaseIdsQuery : IRequest<GetPersonExternDatabaseIdsQueryResponse>
{
	public long PersonId { get; set; }
}

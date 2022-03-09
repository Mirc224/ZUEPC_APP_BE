using MediatR;

namespace ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;

public class GetPersonPersonExternDatabaseIdsQuery : IRequest<GetPersonPersonExternDatabaseIdsQueryResponse>
{
	public long PersonId { get; set; }
}

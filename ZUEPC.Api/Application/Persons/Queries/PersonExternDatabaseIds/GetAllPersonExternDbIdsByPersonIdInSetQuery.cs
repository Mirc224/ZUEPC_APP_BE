using MediatR;

namespace ZUEPC.Api.Application.Persons.Queries.PersonExternDatabaseIds;

public class GetAllPersonExternDbIdsByPersonIdInSetQuery : IRequest<GetAllPersonExternDbIdsByPersonIdInSetQueryResponse>
{
	public IEnumerable<long> PersonIds { get; set; }
}

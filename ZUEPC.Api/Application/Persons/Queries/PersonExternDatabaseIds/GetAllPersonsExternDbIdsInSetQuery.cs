using MediatR;

namespace ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;

public class GetAllPersonsExternDbIdsInSetQuery : IRequest<GetAllPersonsExternDbIdsInSetQueryResponse>
{
	public IEnumerable<string> SearchedExternIdentifiers { get; set; }
}

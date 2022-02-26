using MediatR;
using ZUEPC.Common.Responses;

namespace ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;

public class GetAllPersonsExternDbIdsInSetQuery : IRequest<GetAllPersonsExternDbIdsInSetQueryResponse>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public IEnumerable<string> SearchedExternIdentifiers { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}

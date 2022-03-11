using MediatR;
using ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

namespace ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

public class GetAllPublicationExternDbIdsInSetQuery : IRequest<GetAllPublicationExternDbIdsInSetQueryResponse>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public IEnumerable<string> SearchedExternIdentifiers { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}

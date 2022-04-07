using MediatR;
using ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

namespace ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

public class GetAllPublicationExternDbIdsInSetQuery : IRequest<GetAllPublicationExternDbIdsInSetQueryResponse>
{
	public IEnumerable<string>? SearchedExternIdentifiers { get; set; }
}

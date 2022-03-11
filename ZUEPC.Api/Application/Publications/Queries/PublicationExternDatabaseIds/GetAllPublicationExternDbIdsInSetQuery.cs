using MediatR;
using ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

namespace ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetAllPublicationIdentifiersInSetQuery : IRequest<GetAllPublicationIdentifiersInSetQueryResponse>
{
	public IEnumerable<string>? SearchedIdentifiers { get; set; }
}

using MediatR;

namespace ZUEPC.Application.Publications.Queries;

public class GetAllPublicationIdentifiersInSetQuery : IRequest<GetAllPublicationIdentifiersInSetQueryResponse>
{
	public IEnumerable<string>? SearchedIdentifiers { get; set; }
}

using MediatR;

namespace ZUEPC.Application.Publications.Queries;

public class GetAllPublicationExternDbIdsInSetQuery : IRequest<GetAllPublicationExternDbIdsInSetQueryResponse>
{
	public IEnumerable<string>? SearchedExternIdentifiers { get; set; }
}

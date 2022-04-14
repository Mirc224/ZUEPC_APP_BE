using MediatR;

namespace ZUEPC.Api.Application.Persons.Queries.Persons.Previews;

public class GetAllPersonPreviewsForIdsInSetQuery : IRequest<GetAllPersonPreviewsForIdsInSetQueryResponse>
{
	public IEnumerable<long> PersonIds { get; set; }
}

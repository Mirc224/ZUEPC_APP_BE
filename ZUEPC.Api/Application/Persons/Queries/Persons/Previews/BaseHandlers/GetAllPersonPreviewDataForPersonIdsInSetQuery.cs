using MediatR;

namespace ZUEPC.Api.Application.Persons.Queries.Persons.Previews.BaseHandlers;

public class GetAllPersonPreviewDataForPersonIdsInSetQuery : IRequest<GetAllPersonPreviewDataForPersonIdsInSetQueryResponse>
{
	public IEnumerable<long> PersonIds { get; set; }
}

using MediatR;

namespace ZUEPC.Api.Application.Publications.Queries.Publications.Previews;

public class GetAllPublicationsPreviewDataForPublicationIdsInSetQuery:
	IRequest<GetAllPublicationsPreviewDataForPublicationIdsInSetQueryResponse>
{
	public IEnumerable<long> PublicationIds { get; set; }
}

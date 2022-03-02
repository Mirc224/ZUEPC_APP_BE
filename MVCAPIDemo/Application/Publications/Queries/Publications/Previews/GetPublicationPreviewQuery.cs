using MediatR;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews;

public class GetPublicationPreviewQuery : IRequest<GetPublicationPreviewQueryResponse>
{
	public long PublicationId { get; set; }
}

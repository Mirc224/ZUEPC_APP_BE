using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Common.Responses;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews;

public class GetPublicationPreviewQueryResponse : ResponseBase
{
	public PublicationPreview? PublicationPreview { get; set; }
}
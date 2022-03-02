using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.Common.Responses;

namespace ZUEPC.Application.PublicationAuthors.Queries.Previews;

public class GetPublicationAuthorsPreviewsQueryResponse : ResponseBase
{
	public ICollection<PublicationAuthorDetails> PublicationAuthorPreviews { get; set; }
}
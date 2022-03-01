using ZUEPC.Application.Publications.Entities.Details;
using ZUEPC.Common.Responses;

namespace ZUEPC.Application.Publications.Queries.Publications;

public class GetPublicationWithDetailsQueryResponse : ResponseBase
{
	public PublicationDetails? PublicationWithDetails { get; set; }
}
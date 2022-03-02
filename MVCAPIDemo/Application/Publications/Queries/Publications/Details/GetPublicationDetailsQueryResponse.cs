using ZUEPC.Application.Publications.Entities.Details;
using ZUEPC.Common.Responses;

namespace ZUEPC.Application.Publications.Queries.Publications.Details;

public class GetPublicationDetailsQueryResponse : ResponseBase
{
	public PublicationDetails? PublicationWithDetails { get; set; }
}
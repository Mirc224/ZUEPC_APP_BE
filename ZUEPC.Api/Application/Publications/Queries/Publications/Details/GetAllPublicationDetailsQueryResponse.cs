using ZUEPC.Application.Publications.Entities.Details;
using ZUEPC.Base.Responses;

namespace ZUEPC.Application.Publications.Queries.Publications.Details;

public class GetAllPublicationDetailsQueryResponse
	: PaginatedResponseBase<IEnumerable<PublicationDetails>>
{
}
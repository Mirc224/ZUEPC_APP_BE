using ZUEPC.Application.Publications.Entities.Details;
using ZUEPC.Responses;

namespace ZUEPC.Application.Publications.Queries.Publications.Details;

public class GetAllPublicationDetailsQueryResponse
	: PagedResponseBase<IEnumerable<PublicationDetails>>
{
}
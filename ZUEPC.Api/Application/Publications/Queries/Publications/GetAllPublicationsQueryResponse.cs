using ZUEPC.EvidencePublication.Domain.Publications;
using ZUEPC.Base.Responses;

namespace ZUEPC.Application.Publications.Queries.Publications;

public class GetAllPublicationsQueryResponse :
	PaginatedResponseBase<IEnumerable<Publication>>
{
}
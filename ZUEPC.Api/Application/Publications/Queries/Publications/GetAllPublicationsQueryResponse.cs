using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.Responses;

namespace ZUEPC.Application.Publications.Queries.Publications;

public class GetAllPublicationsQueryResponse :
	PagedResponseBase<IEnumerable<Publication>>
{
}
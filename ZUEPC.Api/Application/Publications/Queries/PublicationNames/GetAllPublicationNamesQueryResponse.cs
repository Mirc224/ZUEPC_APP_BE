using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.Responses;

namespace ZUEPC.Api.Application.Publications.Queries.PublicationNames;

public class GetAllPublicationNamesQueryResponse:
	PagedResponseBase<IEnumerable<PublicationName>>
{
}
using ZUEPC.EvidencePublication.Domain.Publications;
using ZUEPC.Base.Responses;

namespace ZUEPC.Api.Application.Publications.Queries.PublicationNames;

public class GetAllPublicationNamesQueryResponse:
	PaginatedResponseBase<IEnumerable<PublicationName>>
{
}
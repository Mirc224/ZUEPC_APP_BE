using MediatR;
using ZUEPC.Base.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Api.Application.Publications.Queries.PublicationNames;

public class GetAllPublicationNamesQuery:
	PaginatedQueryWithFilterBase<PublicationNameFilter>,
	IRequest<GetAllPublicationNamesQueryResponse>
{
}

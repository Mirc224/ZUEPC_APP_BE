using MediatR;
using ZUEPC.Base.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Publications.Queries.Publications;

public class GetAllPublicationsQuery :
	PaginatedQueryWithFilterBase<PublicationFilter>,
	IRequest<GetAllPublicationsQueryResponse>
{
}

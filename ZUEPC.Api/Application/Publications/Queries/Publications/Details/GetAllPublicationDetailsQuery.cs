using MediatR;
using ZUEPC.Base.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Publications.Queries.Publications.Details;

public class GetAllPublicationDetailsQuery :
	PaginatedQueryWithFilterBase<PublicationFilter>,
	IRequest<GetAllPublicationDetailsQueryResponse>
{
}

using MediatR;
using ZUEPC.Base.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews;

public class GetAllPublicationPreviewsQuery :
	PaginatedQueryWithFilterBase<PublicationFilter>, 
	IRequest<GetAllPublicationPreviewsQueryResponse>
{
}

using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews;

public class GetAllPublicationPreviewsQuery :
	PaginationWithFilterQueryBase<PublicationFilter>, 
	IRequest<GetAllPublicationPreviewsQueryResponse>
{
}

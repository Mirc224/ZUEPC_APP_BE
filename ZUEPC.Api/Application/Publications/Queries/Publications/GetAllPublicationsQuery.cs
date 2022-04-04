using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Publications.Queries.Publications;

public class GetAllPublicationsQuery :
	PaginationWithFilterQueryBase<PublicationFilter>,
	IRequest<GetAllPublicationsQueryResponse>
{
}

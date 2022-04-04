using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Api.Application.Publications.Queries.PublicationNames;

public class GetAllPublicationNamesQuery:
	PaginationWithFilterQueryBase<PublicationNameFilter>,
	IRequest<GetAllPublicationNamesQueryResponse>
{
}

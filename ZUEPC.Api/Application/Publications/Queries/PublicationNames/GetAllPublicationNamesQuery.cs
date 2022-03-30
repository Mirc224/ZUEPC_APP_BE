using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Api.Application.Publications.Queries.PublicationNames;

public class GetAllPublicationNamesQuery:
	PaginationWithFilterQueryBase<PublicationNameFilter>,
	IRequest<GetAllPublicationNamesQueryResponse>
{
}

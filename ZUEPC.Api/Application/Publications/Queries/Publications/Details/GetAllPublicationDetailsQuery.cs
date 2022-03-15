using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Application.Publications.Queries.Publications.Details;

public class GetAllPublicationDetailsQuery :
	PaginationWithFilterQueryBase<PublicationFilter>,
	IRequest<GetAllPublicationDetailsQueryResponse>
{
}

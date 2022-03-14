using MediatR;
using ZUEPC.Common.CQRS.Queries;

namespace ZUEPC.Application.Publications.Queries.Publications;

public class GetAllPublicationsQuery
	: PaginationWithUriQueryBase,
	IRequest<GetAllPublicationsQueryResponse>
{
}

using MediatR;
using ZUEPC.Common.CQRS.Query;

namespace ZUEPC.Application.Publications.Queries.Publications;

public class GetAllPublicationsQuery
	: PaginationQueryWithUriBase,
	IRequest<GetAllPublicationsQueryResponse>
{
}

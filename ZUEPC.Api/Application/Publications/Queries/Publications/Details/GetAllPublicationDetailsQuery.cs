using MediatR;
using ZUEPC.Common.CQRS.Query;

namespace ZUEPC.Application.Publications.Queries.Publications.Details;

public class GetAllPublicationDetailsQuery :
	PaginationQueryWithUriBase,
	IRequest<GetAllPublicationDetailsQueryResponse>
{
}

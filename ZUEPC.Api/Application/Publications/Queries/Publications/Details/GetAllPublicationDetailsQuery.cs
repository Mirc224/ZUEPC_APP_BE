using MediatR;
using ZUEPC.Common.CQRS.Query;

namespace ZUEPC.Application.Publications.Queries.Publications.Details;

public class GetAllPublicationDetailsQuery :
	EPCPaginationQueryWithUriBase,
	IRequest<GetAllPublicationDetailsQueryResponse>
{
}

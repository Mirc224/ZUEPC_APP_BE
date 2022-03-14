using MediatR;
using ZUEPC.Common.CQRS.Queries;

namespace ZUEPC.Application.Publications.Queries.Publications.Details;

public class GetAllPublicationDetailsQuery :
	PaginationWithUriQueryBase,
	IRequest<GetAllPublicationDetailsQueryResponse>
{
}

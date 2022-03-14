using MediatR;
using ZUEPC.Common.CQRS.Queries;

namespace ZUEPC.Application.Persons.Queries.Persons.Details;

public class GetAllPersonDetailsQuery:
	PaginationWithUriQueryBase, 
	IRequest<GetAllPersonDetailsQueryResponse>
{
}

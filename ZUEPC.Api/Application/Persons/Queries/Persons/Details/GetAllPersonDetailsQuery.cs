using MediatR;
using ZUEPC.Common.CQRS.Query;

namespace ZUEPC.Application.Persons.Queries.Persons.Details;

public class GetAllPersonDetailsQuery:
	PaginationQueryWithUriBase, 
	IRequest<GetAllPersonDetailsQueryResponse>
{
}

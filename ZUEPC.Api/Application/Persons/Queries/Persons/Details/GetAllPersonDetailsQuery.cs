using MediatR;
using ZUEPC.Base.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Persons.Queries.Persons.Details;

public class GetAllPersonDetailsQuery:
	PaginatedQueryWithFilterBase<PersonFilter>,
	IRequest<GetAllPersonDetailsQueryResponse>
{
}

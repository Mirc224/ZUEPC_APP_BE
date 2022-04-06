using MediatR;
using ZUEPC.Base.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Persons.Queries.Persons;

public class GetAllPersonsQuery :
	PaginatedQueryWithFilterBase<PersonFilter>,
	IRequest<GetAllPersonsQueryResponse>
{
}

using MediatR;
using ZUEPC.Base.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Api.Application.Persons.Queries.PersonNames;

public class GetAllPersonNamesQuery :
	PaginatedQueryWithFilterBase<PersonNameFilter>,
	IRequest<GetAllPersonNamesQueryResponse>
{
}

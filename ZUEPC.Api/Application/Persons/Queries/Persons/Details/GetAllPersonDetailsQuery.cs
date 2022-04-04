using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Persons.Queries.Persons.Details;

public class GetAllPersonDetailsQuery:
	PaginationWithFilterQueryBase<PersonFilter>,
	IRequest<GetAllPersonDetailsQueryResponse>
{
}

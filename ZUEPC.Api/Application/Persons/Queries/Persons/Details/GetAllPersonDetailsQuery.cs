using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Application.Persons.Queries.Persons.Details;

public class GetAllPersonDetailsQuery:
	PaginationWithFilterQueryBase<PersonFilter>,
	IRequest<GetAllPersonDetailsQueryResponse>
{
}

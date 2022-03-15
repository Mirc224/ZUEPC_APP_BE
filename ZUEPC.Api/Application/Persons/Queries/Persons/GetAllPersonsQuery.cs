using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.Common.CQRS.Queries;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Application.Persons.Queries.Persons;

public class GetAllPersonsQuery :
	PaginationWithFilterQueryBase<PersonFilter>,
	IRequest<GetAllPersonsQueryResponse>
{
}

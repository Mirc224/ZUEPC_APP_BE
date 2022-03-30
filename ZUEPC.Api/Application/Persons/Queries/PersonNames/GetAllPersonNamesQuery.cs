using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.Application.Persons.Queries.Persons;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Api.Application.Persons.Queries.PersonNames;

public class GetAllPersonNamesQuery :
	PaginationWithFilterQueryBase<PersonNameFilter>,
	IRequest<GetAllPersonNamesQueryResponse>
{
}

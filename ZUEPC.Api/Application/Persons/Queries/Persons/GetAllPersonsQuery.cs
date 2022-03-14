using MediatR;
using ZUEPC.Common.CQRS.Queries;

namespace ZUEPC.Application.Persons.Queries.Persons;

public class GetAllPersonsQuery :
	PaginationWithUriQueryBase,
	IRequest<GetAllPersonsQueryResponse>
{
}

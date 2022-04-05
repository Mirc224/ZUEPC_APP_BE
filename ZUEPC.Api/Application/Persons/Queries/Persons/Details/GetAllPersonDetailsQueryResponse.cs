using ZUEPC.Application.Persons.Entities.Details;
using ZUEPC.Base.Responses;

namespace ZUEPC.Application.Persons.Queries.Persons.Details;

public class GetAllPersonDetailsQueryResponse :
	PaginatedResponseBase<IEnumerable<PersonDetails>>
{
}
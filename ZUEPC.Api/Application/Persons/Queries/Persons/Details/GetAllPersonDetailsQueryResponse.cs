using ZUEPC.Application.Persons.Entities.Details;
using ZUEPC.Responses;

namespace ZUEPC.Application.Persons.Queries.Persons.Details;

public class GetAllPersonDetailsQueryResponse :
	PagedResponseBase<IEnumerable<PersonDetails>>
{
}
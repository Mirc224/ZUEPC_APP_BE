using ZUEPC.EvidencePublication.Domain.Persons;
using ZUEPC.Base.Responses;

namespace ZUEPC.Application.Persons.Queries.Persons;

public class GetAllPersonsQueryResponse : 
	PaginatedResponseBase<IEnumerable<Person>>
{
}

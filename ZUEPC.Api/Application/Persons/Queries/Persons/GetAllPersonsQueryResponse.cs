using ZUEPC.EvidencePublication.Domain.Persons;
using ZUEPC.Responses;

namespace ZUEPC.Application.Persons.Queries.Persons;

public class GetAllPersonsQueryResponse : 
	PagedResponseBase<IEnumerable<Person>>
{
}

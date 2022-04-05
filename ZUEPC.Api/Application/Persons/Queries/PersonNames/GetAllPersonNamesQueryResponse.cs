using ZUEPC.EvidencePublication.Domain.Persons;
using ZUEPC.Base.Responses;

namespace ZUEPC.Api.Application.Persons.Queries.PersonNames;

public class GetAllPersonNamesQueryResponse :
	PaginatedResponseBase<IEnumerable<PersonName>>
{
}
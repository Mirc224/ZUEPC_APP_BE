using ZUEPC.EvidencePublication.Base.Domain.Persons;
using ZUEPC.Responses;

namespace ZUEPC.Api.Application.Persons.Queries.PersonNames;

public class GetAllPersonNamesQueryResponse :
	PagedResponseBase<IEnumerable<PersonName>>
{
}
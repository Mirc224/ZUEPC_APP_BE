using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.PersonNames;

public class GetPersonNamesQueryResponse: ResponseBase
{
	public IEnumerable<PersonName> PersonNames { get; set; }
}
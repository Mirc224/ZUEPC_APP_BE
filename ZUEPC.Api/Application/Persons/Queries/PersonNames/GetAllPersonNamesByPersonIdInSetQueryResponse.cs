using ZUEPC.Base.Responses;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Api.Application.Persons.Queries.PersonNames;

public class GetAllPersonNamesByPersonIdInSetQueryResponse : ResponseWithDataBase<IEnumerable<PersonName>>
{
}
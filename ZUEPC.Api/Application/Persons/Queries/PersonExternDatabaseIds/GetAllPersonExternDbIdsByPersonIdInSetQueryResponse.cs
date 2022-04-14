using ZUEPC.Base.Responses;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Api.Application.Persons.Queries.PersonExternDatabaseIds;

public class GetAllPersonExternDbIdsByPersonIdInSetQueryResponse : ResponseWithDataBase<IEnumerable<PersonExternDatabaseId>>
{
}
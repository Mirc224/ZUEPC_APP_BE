using ZUEPC.Responses;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;

public class GetPersonPersonExternDatabaseIdsQueryResponse : ResponseWithDataBase<ICollection<PersonExternDatabaseId>>
{
}
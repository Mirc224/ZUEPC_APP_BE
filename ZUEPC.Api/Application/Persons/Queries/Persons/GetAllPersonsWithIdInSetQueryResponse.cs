using ZUEPC.Base.Responses;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Api.Application.Persons.Queries.Persons;

public class GetAllPersonsWithIdInSetQueryResponse : ResponseWithDataBase<IEnumerable<Person>>
{
}
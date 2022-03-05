using ZUEPC.Application.Persons.Entities.Details;
using ZUEPC.Common.Responses;

namespace ZUEPC.Application.Persons.Queries.Persons.Details;

public class GetPersonDetailsQueryResponse : ResponseBase
{
	public PersonDetails? PersonDetails { get; set; }
}
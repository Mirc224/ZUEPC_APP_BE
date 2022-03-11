using ZUEPC.Application.Persons.Entities.Details;
using ZUEPC.Responses;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class CreatePersonWithDetailsCommandResponse : ResponseBase
{
	public PersonDetails CreatedPersonDetails { get; set; }
}
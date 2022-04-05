using ZUEPC.Application.Persons.Entities.Details;
using ZUEPC.Base.Responses;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class CreatePersonWithDetailsCommandResponse : ResponseBase
{
	public PersonDetails CreatedPersonDetails { get; set; }
}
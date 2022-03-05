using ZUEPC.Application.Persons.Entities.Details;
using ZUEPC.Common.Responses;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class CreatePersonWithDetailsCommandResponse : ResponseBase
{
	public PersonDetails CreatedPersonDetails { get; set; }
}
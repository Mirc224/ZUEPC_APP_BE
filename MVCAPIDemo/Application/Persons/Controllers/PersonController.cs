using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZUEPC.Application.Persons.Commands.Persons;
using ZUEPC.Application.Persons.Queries.Persons.Details;
using ZUEPC.Base.Enums.Common;

namespace ZUEPC.Application.Persons.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
	private readonly IMediator _mediator;

	public PersonController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetPersonDetails(long id)
	{
		GetPersonDetailsQuery request = new() { PersonId = id };
		GetPersonDetailsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response.Data);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeletePerson(long id)
	{
		DeletePersonCommand request = new() { Id = id };
		DeletePersonCommandResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return NoContent();
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdatePerson([FromBody]UpdatePersonWithDetailsCommand request, [FromRoute] long id)
	{
		request.Id = id;
		request.OriginSourceType = OriginSourceType.ZUEPC;
		request.VersionDate = DateTime.Now;
		UpdatePersonWithDetailsCommandResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok();
	}

	[HttpPost]
	public async Task<IActionResult> CreatePerson(CreatePersonWithDetailsCommand request)
	{
		request.OriginSourceType = OriginSourceType.ZUEPC;
		request.VersionDate = DateTime.Now;
		CreatePersonWithDetailsCommandResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return BadRequest();
		}
		return StatusCode(201, response.CreatedPersonDetails);
	}
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZUEPC.Application.Institutions.Commands.Institutions;
using ZUEPC.Application.Institutions.Queries.Institutions.Details;
using ZUEPC.Application.Persons.Commands.Persons;
using ZUEPC.Base.Enums.Common;

namespace ZUEPC.Application.Institutions.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstitutionController : ControllerBase
{
	private readonly IMediator _mediator;

	public InstitutionController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetInstitutionDetails(long id)
	{
		GetInstitutionDetailsQuery request = new() { InstitutionId = id };
		GetInstitutionDetailsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response.InstitutionDetails);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteInstitution(long id)
	{
		DeleteInstitutionCommand request = new() { InstitutionId = id };
		DeleteInstitutionCommandResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return BadRequest();
		}
		return NoContent();
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdatePerson([FromBody] UpdateInstitutionWithDetailsCommand request, [FromRoute] long id)
	{
		request.Id = id;
		request.OriginSourceType = OriginSourceType.ZUEPC;
		request.VersionDate = DateTime.Now;
		UpdateInstitutionWithDetailsCommandResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok();
	}

	[HttpPost]
	public async Task<IActionResult> CreateInstitution(CreateInstitutionWithDetailsCommand request)
	{
		request.OriginSourceType = OriginSourceType.ZUEPC;
		request.VersionDate = DateTime.Now;
		CreateInstitutionWithDetailsCommandResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return BadRequest();
		}
		return StatusCode(201, response.CreatedInstitutionDetails);
	}
}
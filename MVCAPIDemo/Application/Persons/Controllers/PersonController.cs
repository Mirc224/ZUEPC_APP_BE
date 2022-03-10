using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZUEPC.Application.Persons.Commands.Persons;
using ZUEPC.Application.Persons.Queries.Persons;
using ZUEPC.Application.Persons.Queries.Persons.Details;
using ZUEPC.Application.Persons.Queries.Persons.Previews;
using ZUEPC.Base.Enums.Common;
using ZUEPC.Common.Services.URIServices;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Application.Persons.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly IUriService _uriService;

	public PersonController(IMediator mediator, IUriService uriService)
	{
		_mediator = mediator;
		_uriService = uriService;
	}


	[HttpGet]
	public async Task<IActionResult> GetAll([FromQuery] PaginationFilter? filter)
	{
		string? route = Request.Path.Value;
		GetAllPersonsQuery request = new() { PaginationFilter = filter, UriService = _uriService, Route = route};
		GetAllPersonsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[HttpGet("preview")]
	public async Task<IActionResult> GetAllPreview([FromQuery] PaginationFilter? filter)
	{
		string? route = Request.Path.Value;
		GetAllPersonPreviewsQuery request = new() { PaginationFilter = filter, UriService = _uriService, Route = route };
		GetAllPersonPreviewsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
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
		if (response.ErrorMessages != null && response.ErrorMessages.Any())
		{
			return BadRequest(new { errors = response.ErrorMessages });
		}

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

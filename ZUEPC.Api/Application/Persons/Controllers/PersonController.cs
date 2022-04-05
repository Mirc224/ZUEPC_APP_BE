using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZUEPC.Api.Application.Persons.Queries.PersonNames;
using ZUEPC.Application.Persons.Commands.Persons;
using ZUEPC.Application.Persons.Queries.Persons;
using ZUEPC.Application.Persons.Queries.Persons.Details;
using ZUEPC.Application.Persons.Queries.Persons.Previews;
using ZUEPC.Base.Enums.Common;
using ZUEPC.Base.Services;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Persons.Controllers;

[ApiController]
[Authorize]
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
	public async Task<IActionResult> GetAll([FromQuery] PersonFilter personFilter, [FromQuery] PaginationFilter filter)
	{
		string? route = Request.Path.Value;
		GetAllPersonsQuery request = new() 
		{ 
			PaginationFilter = filter, 
			UriService = _uriService, 
			Route = route, 
			QueryFilter = personFilter
		};
		GetAllPersonsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[HttpGet("preview")]
	public async Task<IActionResult> GetAllPreview([FromQuery] PersonFilter personFilter, [FromQuery] PaginationFilter filter)
	{
		string? route = Request.Path.Value;
		GetAllPersonPreviewsQuery request = new() { PaginationFilter = filter, UriService = _uriService, Route = route, QueryFilter = personFilter};
		GetAllPersonPreviewsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[HttpGet("detail")]
	public async Task<IActionResult> GetAllDetails([FromQuery]PersonFilter personFilter, [FromQuery] PaginationFilter filter)
	{
		string? route = Request.Path.Value;
		GetAllPersonDetailsQuery request = new() { PaginationFilter = filter, UriService = _uriService, Route = route, QueryFilter = personFilter };
		GetAllPersonDetailsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[HttpGet("{id}/preview")]
	public async Task<IActionResult> GetPersonPreview(long id)
	{
		GetPersonPreviewQuery request = new() { Id = id };
		GetPersonPreviewQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response.Data);
	}

	[HttpGet("{id}/detail")]
	public async Task<IActionResult> GetPersonDetails(long id)
	{
		GetPersonDetailsQuery request = new() { Id = id };
		GetPersonDetailsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response.Data);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetPerson(long id)
	{
		GetPersonQuery request = new() { Id = id };
		GetPersonQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response.Data);
	}

	[HttpGet("name")]
	public async Task<IActionResult> GetAllPersonNames([FromQuery] PersonNameFilter personNameFilter, [FromQuery] PaginationFilter filter)
	{
		string? route = Request.Path.Value;
		GetAllPersonNamesQuery request = new()
		{
			PaginationFilter = filter,
			UriService = _uriService,
			Route = route,
			QueryFilter = personNameFilter
		};
		GetAllPersonNamesQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[Authorize(Roles = "EDITOR,ADMIN")]
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

	[Authorize(Roles = "EDITOR,ADMIN")]
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
		return NoContent();
	}

	[Authorize(Roles = "EDITOR,ADMIN")]
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
		return Ok(response.CreatedPersonDetails);
	}
}

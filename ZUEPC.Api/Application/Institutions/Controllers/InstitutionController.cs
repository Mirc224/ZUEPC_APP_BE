using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZUEPC.Api.Application.Institutions.Queries.InstitutionNames;
using ZUEPC.Application.Institutions.Commands.Institutions;
using ZUEPC.Application.Institutions.Queries.Institutions;
using ZUEPC.Application.Institutions.Queries.Institutions.Details;
using ZUEPC.Application.Institutions.Queries.Institutions.Previews;
using ZUEPC.Base.Enums.Common;
using ZUEPC.Common.Services.URIServices;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Application.Institutions.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class InstitutionController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly IUriService _uriService;

	public InstitutionController(IMediator mediator, IUriService uriService)
	{
		_mediator = mediator;
		_uriService = uriService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll([FromQuery]InstitutionFilter? institutionFilter, [FromQuery] PaginationFilter? filter)
	{
		string? route = Request.Path.Value;
		GetAllInstitutionsQuery request = new() 
		{ 
			PaginationFilter = filter, 
			UriService = _uriService, 
			Route = route, 
			QueryFilter = institutionFilter 
		};
		GetAllInstitutionsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[HttpGet("preview")]
	public async Task<IActionResult> GetAllPreview([FromQuery]InstitutionFilter? institutionFilter, [FromQuery] PaginationFilter? filter)
	{
		string? route = Request.Path.Value;
		GetAllInstitutionPreviewsQuery request = new() 
		{ 
			PaginationFilter = filter, 
			UriService = _uriService, 
			Route = route ,
			QueryFilter = institutionFilter
		};
		GetAllInstitutionPreviewsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[HttpGet("detail")]
	public async Task<IActionResult> GetAllDetails([FromQuery] InstitutionFilter? institutionFilter, [FromQuery] PaginationFilter? filter)
	{
		string? route = Request.Path.Value;
		GetAllInstitutionDetailsQuery request = new() 
		{ 
			PaginationFilter = filter, 
			UriService = _uriService, 
			Route = route,
			QueryFilter = institutionFilter
		};
		GetAllInstitutionDetailsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[HttpGet("{id}/preview")]
	public async Task<IActionResult> GetInstitutionPreview(long id)
	{
		GetInstitutionPreviewQuery request = new() { Id = id };
		GetInstitutionPreviewQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response.Data);
	}

	[HttpGet("{id}/detail")]
	public async Task<IActionResult> GetInstitutionDetails(long id)
	{
		GetInstitutionDetailsQuery request = new() { Id = id };
		GetInstitutionDetailsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response.Data);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetInstitution(long id)
	{
		GetInstitutionQuery request = new() { Id = id };
		GetInstitutionQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response.Data);
	}

	[HttpGet("name")]
	public async Task<IActionResult> GetAllInstitutionNames([FromQuery] InstitutionNameFilter? institutionNameFilter, [FromQuery] PaginationFilter? filter)
	{
		string? route = Request.Path.Value;
		GetAllInstitutionNamesQuery request = new()
		{
			PaginationFilter = filter,
			UriService = _uriService,
			Route = route,
			QueryFilter = institutionNameFilter
		};
		GetAllInstitutionNamesQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[Authorize(Roles = "EDITOR,ADMIN")]
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

	[Authorize(Roles = "EDITOR,ADMIN")]
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdatePerson([FromBody] UpdateInstitutionWithDetailsCommand request, [FromRoute] long id)
	{
		request.Id = id;
		request.OriginSourceType = OriginSourceType.ZUEPC;
		request.VersionDate = DateTime.Now;
		UpdateInstitutionWithDetailsCommandResponse response = await _mediator.Send(request);
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
	public async Task<IActionResult> CreateInstitution(CreateInstitutionWithDetailsCommand request)
	{
		request.OriginSourceType = OriginSourceType.ZUEPC;
		request.VersionDate = DateTime.Now;
		CreateInstitutionWithDetailsCommandResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return BadRequest();
		}
		return Ok(response.CreatedInstitutionDetails);
	}
}
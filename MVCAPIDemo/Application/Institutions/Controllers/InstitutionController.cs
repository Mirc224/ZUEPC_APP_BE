using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZUEPC.Application.Institutions.Commands.Institutions;
using ZUEPC.Application.Institutions.Queries.Institutions;
using ZUEPC.Application.Institutions.Queries.Institutions.Details;
using ZUEPC.Application.Institutions.Queries.Institutions.Previews;
using ZUEPC.Base.Enums.Common;
using ZUEPC.Common.Services.URIServices;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Application.Institutions.Controllers;

[ApiController]
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
	public async Task<IActionResult> GetAll([FromQuery] PaginationFilter? filter)
	{
		string? route = Request.Path.Value;
		GetAllInstitutionsQuery request = new() { PaginationFilter = filter, UriService = _uriService, Route = route };
		GetAllInstitutionsQueryResponse response = await _mediator.Send(request);
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
		GetAllInstitutionPreviewsQuery request = new() { PaginationFilter = filter, UriService = _uriService, Route = route };
		GetAllInstitutionPreviewsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
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
		return Ok(response.Data);
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
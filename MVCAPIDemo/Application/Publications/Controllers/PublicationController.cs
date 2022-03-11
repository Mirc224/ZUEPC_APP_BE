using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZUEPC.Application.Publications.Commands.Publications;
using ZUEPC.Application.Publications.Queries.Publications;
using ZUEPC.Application.Publications.Queries.Publications.Details;
using ZUEPC.Application.Publications.Queries.Publications.Previews;
using ZUEPC.Application.Publications.Queries.Publictions;
using ZUEPC.Base.Enums.Common;
using ZUEPC.Common.Services.URIServices;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Application.Publications.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublicationController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly IUriService _uriService;

	public PublicationController(IMediator mediator, IUriService uriService)
	{
		_mediator = mediator;
		_uriService = uriService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll([FromQuery] PaginationFilter? filter)
	{
		string? route = Request.Path.Value;
		GetAllPublicationsQuery request = new() { PaginationFilter = filter, UriService = _uriService, Route = route };
		GetAllPublicationsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[HttpGet("preview")]
	public async Task<IActionResult> GetAllPreviews([FromQuery] PaginationFilter? filter)
	{
		string? route = Request.Path.Value;
		GetAllPublicationPreviewsQuery request = new() { PaginationFilter = filter, UriService = _uriService, Route = route };
		GetAllPublicationPreviewsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[HttpGet("detail")]
	public async Task<IActionResult> GetAllDetails([FromQuery] PaginationFilter? filter)
	{
		string? route = Request.Path.Value;
		GetAllPublicationDetailsQuery request = new() { PaginationFilter = filter, UriService = _uriService, Route = route };
		GetAllPublicationDetailsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[HttpGet("{id}/preview")]
	public async Task<IActionResult> GetPublicationPreview(long id)
	{
		GetPublicationPreviewQuery request = new() { Id = id };
		GetPublicationPreviewQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response.Data);
	}

	[HttpGet("{id}/detail")]
	public async Task<IActionResult> GetPublicationDetails(long id)
	{
		GetPublicationDetailsQuery request = new() { Id = id };
		GetPublicationDetailsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response.Data);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetPublication(long id)
	{
		GetPublicationQuery request = new() { Id = id };
		GetPublicationQueryResponse response = await _mediator.Send(request);
		if(!response.Success)
		{
			return NotFound();
		}
		return Ok(response.Data);
	}

	[HttpPost]
	public async Task<IActionResult> CreateInstitution(CreatePublicationWithDetailsCommand request)
	{
		request.OriginSourceType = OriginSourceType.ZUEPC;
		request.VersionDate = DateTime.Now;
		CreatePublicationWithDetailsCommandResponse response = await _mediator.Send(request);
		if (response.ErrorMessages != null && response.ErrorMessages.Any())
		{
			return BadRequest(new {errors = response.ErrorMessages});
		}

		if (!response.Success)
		{
			return StatusCode(500);
		}
		return StatusCode(201, response.Data);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdatePublication([FromBody] UpdatePublicationWithDetailsCommand request, [FromRoute] long id)
	{
		request.Id = id;
		request.OriginSourceType = OriginSourceType.ZUEPC;
		request.VersionDate = DateTime.Now;
		UpdatePublicationWithDetailsCommandResponse response = await _mediator.Send(request);
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
}

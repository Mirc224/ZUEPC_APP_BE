using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZUEPC.Api.Application.Publications.Queries.PublicationNames;
using ZUEPC.Application.Publications.Commands.Publications;
using ZUEPC.Application.Publications.Queries.Publications;
using ZUEPC.Application.Publications.Queries.Publications.Details;
using ZUEPC.Application.Publications.Queries.Publications.Previews;
using ZUEPC.Application.Publications.Queries.Publictions;
using ZUEPC.Base.Enums.Common;
using ZUEPC.Base.Services;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Publications.Controllers;

[ApiController]
[Authorize]
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
	public async Task<IActionResult> GetAll([FromQuery]PublicationFilter publicationFilter, [FromQuery] PaginationFilter? filter)
	{
		string? route = Request.Path.Value;
		GetAllPublicationsQuery request = new() { 
			PaginationFilter = filter, 
			UriService = _uriService, 
			Route = route,
			QueryFilter = publicationFilter
		};
		GetAllPublicationsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[HttpGet("preview")]
	public async Task<IActionResult> GetAllPreviews([FromQuery] PublicationFilter publicationFilter, [FromQuery] PaginationFilter? filter)
	{
		string? route = Request.Path.Value;
		GetAllPublicationPreviewsQuery request = new() { 
			PaginationFilter = filter, 
			UriService = _uriService,
			Route = route,
			QueryFilter = publicationFilter
		};
		GetAllPublicationPreviewsQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[HttpGet("detail")]
	public async Task<IActionResult> GetAllDetails([FromQuery] PublicationFilter publicationFilter, [FromQuery] PaginationFilter? filter)
	{
		string? route = Request.Path.Value;
		GetAllPublicationDetailsQuery request = new() { 
			PaginationFilter = filter, 
			UriService = _uriService, 
			Route = route,
			QueryFilter = publicationFilter
		};
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

	[HttpGet("name")]
	public async Task<IActionResult> GetAllPublicationNames([FromQuery] PublicationNameFilter publicationNameFilter, [FromQuery] PaginationFilter? filter)
	{
		string? route = Request.Path.Value;
		GetAllPublicationNamesQuery request = new()
		{
			PaginationFilter = filter,
			UriService = _uriService,
			Route = route,
			QueryFilter = publicationNameFilter
		};
		GetAllPublicationNamesQueryResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return NotFound();
		}
		return Ok(response);
	}

	[Authorize(Roles = "EDITOR,ADMIN")]
	[HttpPost]
	public async Task<IActionResult> CreatePublication(CreatePublicationWithDetailsCommand request)
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
		return Ok(response.Data);
	}

	[Authorize(Roles = "EDITOR,ADMIN")]
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
		return NoContent();
	}

	[Authorize(Roles = "EDITOR,ADMIN")]
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeletePublication(long id)
	{
		DeletePublicationCommand request = new() { Id = id };
		DeletePublicationCommandResponse response = await _mediator.Send(request);
		if (!response.Success)
		{
			return BadRequest();
		}
		return NoContent();
	}
}

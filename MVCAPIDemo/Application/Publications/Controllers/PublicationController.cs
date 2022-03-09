using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZUEPC.Application.Publications.Commands.Publications;
using ZUEPC.Application.Publications.Queries.Publications.Details;
using ZUEPC.Base.Enums.Common;

namespace ZUEPC.Application.Publications.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublicationController : ControllerBase
{
	private readonly IMediator _mediator;

	public PublicationController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetPublicationDetails(long id)
	{
		GetPublicationDetailsQuery request = new() { PublicationId = id };
		GetPublicationDetailsQueryResponse response = await _mediator.Send(request);
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

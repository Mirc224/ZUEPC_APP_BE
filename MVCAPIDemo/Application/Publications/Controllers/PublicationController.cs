using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZUEPC.Application.Publications.Queries.Publications;

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

	[HttpGet("publication/{id}")]
	public async Task<IActionResult> GetPublicationDetails([FromRoute] long publicationId)
	{
		GetPublicationWithDetailsQuery request = new() { PublicationId = publicationId };
		GetPublicationWithDetailsQueryResponse response = await _mediator.Send(request);
		if(!response.Success)
		{
			return NotFound();
		}

		return Ok(response.PublicationWithDetails);
	}
}

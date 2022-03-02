using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZUEPC.Application.Publications.Queries.Publications.Details;

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
		return Ok(response.PublicationWithDetails);
	}
}

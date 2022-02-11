using MediatR;
using Microsoft.AspNetCore.Mvc;
using MVCAPIDemo.Application.Commands.Auth;

namespace MVCAPIDemo.Application.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{

		private readonly IMediator _mediator;

		public AuthController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("register")]
		public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand request)
		{
			var response = await _mediator.Send(request);

			if (!response.Success)
			{
				return BadRequest(
					new
					{
						error = response.ErrorMessages
					});
			}

			return NoContent();
		}

		[HttpPost("login")]
		public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand request)
		{
			var response = await _mediator.Send(request);
			if (!response.Success)
			{
				return Unauthorized(
					new
					{
						error = response.ErrorMessages
					});
			}
			return Ok(new { response.Token, response.RefreshToken });
		}

		[HttpPost("refreshToken")]
		public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand request)
		{
			var response = await _mediator.Send(request);
			if (!response.Success)
			{
				return BadRequest(
					new
					{
						error = response.ErrorMessages
					});
			}
			return Ok(new { response.Token, response.RefreshToken });
		}
	}
}
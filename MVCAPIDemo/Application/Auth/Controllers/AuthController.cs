using Constants.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCAPIDemo.Auth.Commands;
using System.IdentityModel.Tokens.Jwt;

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

			return StatusCode(201, response.CreatedUser);
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

		[HttpPost("logout")]
		[Authorize]
		public async Task<IActionResult> LogoutUser()
		{
			var jwtId = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
			var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == CustomClaims.UserId).Value);
			
			if (jwtId is null)
			{
				return Unauthorized();
			}

			var request = new LogoutUserCommand() { JwtId = jwtId, UserId = userId};
			var response = await _mediator.Send(request);
			if (!response.Success)
			{
				return Unauthorized(
					new
					{
						error = response.ErrorMessages
					});
			}
			return NoContent();
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

		[HttpPost("revokeToken")]
		public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenCommand request)
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
			return Ok();
		}
	}
}
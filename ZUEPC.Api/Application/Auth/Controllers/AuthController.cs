using Constants.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using ZUEPC.Application.Auth.Commands.RefreshTokens;
using ZUEPC.Application.Auth.Commands.AuthActions;

namespace ZUEPC.Application.Auth.Controllers
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
			RegisterUserCommandResponse response = await _mediator.Send(request);

			if (!response.Success)
			{
				return BadRequest(
					new
					{
						error = response.ErrorMessages
					});
			}

			return StatusCode(201, response.Data);
		}

		[HttpPost("login")]
		public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand request)
		{
			LoginUserCommandResponse response = await _mediator.Send(request);
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


		[Authorize]
		[HttpPost("logout")]
		public async Task<IActionResult> LogoutUser()
		{
			string jwtId = User?.Claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
			if (jwtId is null)
			{
				return Unauthorized();
			}
			System.Security.Claims.Claim? userIdClaim = User?.Claims.FirstOrDefault(x => x.Type == CustomClaims.UserId);
			if (userIdClaim is null)
			{
				return Unauthorized();
			}
			int userId = int.Parse(userIdClaim.Value);

			LogoutUserCommand?request = new() { JwtId = jwtId, UserId = userId };
			LogoutUserCommandResponse response = await _mediator.Send(request);
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
			RefreshTokenCommandResponse response = await _mediator.Send(request);
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
		public async Task<IActionResult> RevokeToken([FromBody] RevokeRefreshTokenCommand request)
		{
			RevokeRefreshTokenCommandResponse response = await _mediator.Send(request);
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
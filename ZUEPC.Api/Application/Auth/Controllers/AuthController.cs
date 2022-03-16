using Constants.Infrastructure;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using Users.Base.Domain;
using ZUEPC.Application.Auth.Commands.RefreshTokens;
using ZUEPC.Application.Auth.Commands.Users;

namespace ZUEPC.Application.Auth.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{

		private readonly IMediator _mediator;
		private readonly IConfiguration config;

		public AuthController(IMediator mediator, IConfiguration config)
		{
			_mediator = mediator;
			this.config = config;
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

		[HttpGet("testsss")]
		public async Task<IActionResult> Test()
		{
			using(IDbConnection connection = new SqlConnection(config.GetConnectionString("default")))
			{
				try
				{
					connection.Open();
					return Ok();
				}
				catch(SqlException ex)
				{
					return BadRequest(new { message = ex.Message, code = ex.ErrorCode, erros = ex.Errors, conn = config.GetConnectionString("default") });
				}
			}
			
			//return Ok(await connection.QueryAsync<User>("Select * from Users", commandType: CommandType.Text))

			//return Ok(new { Conn = config.GetConnectionString("default")});
		}

		[HttpPost("logout")]
		[Authorize]
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
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZUEPC.Api.Application.Users.Commands.UserRoles;
using ZUEPC.Api.Application.Users.Commands.Users;
using ZUEPC.Api.Application.Users.Queries.Users;
using ZUEPC.Api.Application.Users.Queries.Users.Details;
using ZUEPC.Api.Common.Authorization;
using ZUEPC.Application.Users.Queries.Roles;
using ZUEPC.Base.QueryFilters;
using ZUEPC.Base.Services;

namespace ZUEPC.Application.Users.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{

		private readonly IMediator _mediator;
		private readonly IUriService _uriService;

		public UserController(IMediator mediator, IUriService uriService)
		{
			_mediator = mediator;
			_uriService = uriService;
		}

		[Authorize(Roles = "ADMIN")]
		[HttpGet("detail")]
		public async Task<IActionResult> GetUsersDetails([FromQuery] UserFilter userFilter, [FromQuery] PaginationFilter paginationFilter)
		{
			string? route = Request.Path.Value;
			GetAllUsersDetailsQuery request = new() 
			{ 
				QueryFilter = userFilter,
				PaginationFilter = paginationFilter, 
				UriService = _uriService, 
				Route = route 
			};
			GetAllUsersDetailsQueryResponse response = await _mediator.Send(request);
			if (!response.Success)
				return StatusCode(500);
			return Ok(response);
		}

		[Authorize(Roles = "ADMIN")]
		[HttpGet]
		public async Task<IActionResult> GetUsers([FromQuery] PaginationFilter filter)
		{
			string? route = Request.Path.Value;
			GetAllUsersQuery request = new() { PaginationFilter = filter, UriService = _uriService, Route = route };
			GetAllUsersQueryResponse response = await _mediator.Send(request);
			if (!response.Success)
				return StatusCode(500);
			return Ok(response);
		}

		[UserAuthorization]
		[HttpGet("{userId}/detail")]
		public async Task<IActionResult> GetUserDetails(long userId)
		{
			GetUserDetailsQuery? query = new() { Id = userId };
			GetUserDetailsQueryResponse? response = await _mediator.Send(query);
			if (!response.Success)
			{
				return NotFound(
					new
					{
						errors = response.ErrorMessages
					});
			}

			return Ok(response.Data);
		}

		[Authorize(Roles = "ADMIN")]
		[HttpPut("{userId}/role")]
		public async Task<IActionResult> UpdateUserRoles([FromBody]UpdateUserRolesCommand request, [FromRoute]long userId)
		{
			request.UserId = userId;
			UpdateUserRolesCommandResponse response = await _mediator.Send(request);
			if (!response.Success)
				return StatusCode(500);
			return NoContent();
		}

		[Authorize(Roles = "ADMIN")]
		[HttpGet("role")]
		public async Task<IActionResult> GetRoles()
		{
			GetAllRolesQueryResponse response = await _mediator.Send(new GetAllRolesQuery());
			if (!response.Success)
				return StatusCode(500);
			return Ok(response.Roles);
		}

		[Authorize(Roles = "ADMIN")]
		[HttpDelete("{userId}")]
		public async Task<IActionResult> DeleteUser(long userId)
		{
			DeleteUserCommandResponse response = await _mediator.Send(new DeleteUserCommand() { Id = userId});
			if (!response.Success)
				return NotFound();
			return NoContent();
		}

		[UserAuthorization]
		[HttpPut("{userId}")]
		public async Task<IActionResult> UpdateUser([FromBody]UpdateUserCommand request, [FromRoute]long userId)
		{
			request.Id = userId;
			UpdateUserCommandResponse response = await _mediator.Send(request);
			if (!response.Success)
				return NotFound();
			return NoContent();
		}
	}
}
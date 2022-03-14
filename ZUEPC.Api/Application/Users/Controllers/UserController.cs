using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Users.Base.Domain;
using ZUEPC.Api.Application.Users.Commands.UserRoles;
using ZUEPC.Api.Application.Users.Commands.Users;
using ZUEPC.Api.Application.Users.Queries.Users;
using ZUEPC.Api.Application.Users.Queries.Users.Details;
using ZUEPC.Application.Users.Commands;
using ZUEPC.Application.Users.Queries;
using ZUEPC.Application.Users.Validators;
using ZUEPC.Common.Services.URIServices;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Application.Users.Controllers
{
	[ApiController]
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

		[HttpGet("details")]
		//[Authorize(Roles = "ADMIN")]
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

		[HttpGet]
		//[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> GetUsers([FromQuery] PaginationFilter filter)
		{
			string? route = Request.Path.Value;
			GetAllUsersQuery request = new() { PaginationFilter = filter, UriService = _uriService, Route = route };
			GetAllUsersQueryResponse response = await _mediator.Send(request);
			if (!response.Success)
				return StatusCode(500);
			return Ok(response);
		}

		[HttpGet("{id}/details")]
		public async Task<IActionResult> GetUserDetails(long id)
		{
			GetUserDetailsQuery? query = new() { Id = id };
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

		[HttpPut("{id}/role")]
		public async Task<IActionResult> UpdateUserRoles([FromBody]UpdateUserRolesCommand request, [FromRoute]long id)
		{
			request.UserId = id;
			UpdateUserRolesCommandResponse response = await _mediator.Send(request);
			if (!response.Success)
				return StatusCode(500);
			return Ok();
		}

		[HttpPost("roles")]
		public async Task<IActionResult> GetRoles()
		{
			GetAllRolesQueryResponse response = await _mediator.Send(new GetAllRolesQuery());
			if (!response.Success)
				return StatusCode(500);
			return Ok(response.Roles);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(long id)
		{
			DeleteUserCommandResponse response = await _mediator.Send(new DeleteUserCommand() { Id = id});
			if (!response.Success)
				return NotFound();
			return NoContent();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUser([FromBody]UpdateUserCommand request, [FromRoute]long id)
		{
			request.Id = id;
			UpdateUserCommandResponse response = await _mediator.Send(request);
			if (!response.Success)
				return NotFound();
			return NoContent();
		}

		[HttpPatch("{id}")]
		public async Task<IActionResult> PatchUser([FromBody] JsonPatchDocument<User> patchEntity, int id)
		{
			User user = new();
			patchEntity.ApplyTo(user);

			FluentValidation.Results.ValidationResult? validationResult = new UserValidator().Validate(user);
			if (!validationResult.IsValid)
			{
				foreach (FluentValidation.Results.ValidationFailure? error in validationResult.Errors)
				{
					ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
				}
				return UnprocessableEntity(ModelState);
			}
			PatchUserCommand? request = new PatchUserCommand
			{
				AppliedPatch = patchEntity,
				UserId = id
			};

			PatchUserCommandResponse? response = await _mediator.Send(request);
			if (!response.Success)
				return BadRequest();
			return NoContent();
		}
	}
}
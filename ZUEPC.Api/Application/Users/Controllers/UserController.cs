using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Users.Base.Domain;
using ZUEPC.Api.Application.Users.Queries.Users.Details;
using ZUEPC.Application.Users.Commands;
using ZUEPC.Application.Users.Queries;
using ZUEPC.Application.Users.Validators;

namespace ZUEPC.Application.Users.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{

		private readonly IMediator _mediator;

		public UserController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		//[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> GetUsers()
		{
			ClaimsPrincipal claimsPrincipal = User;
			GetAllUsersQueryResponse response = await _mediator.Send(new GetAllUsersQuery());
			if (!response.Success)
				return StatusCode(500);
			return Ok(response.Users);
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

		[HttpPost("roles")]
		public async Task<IActionResult> GetRoles()
		{
			GetAllRolesQueryResponse response = await _mediator.Send(new GetAllRolesQuery());
			if (!response.Success)
				return StatusCode(500);
			return Ok(response.Roles);
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
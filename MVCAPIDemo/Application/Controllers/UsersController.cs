using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MVCAPIDemo.Application.Commands.Users;
using MVCAPIDemo.Application.Domain;
using MVCAPIDemo.Application.Queries.Users;
using MVCAPIDemo.Application.Validators.Users;

namespace MVCAPIDemo.Application.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : ControllerBase
	{

		private readonly IMediator _mediator;

		public UsersController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		//[Authorize]
		public async Task<IActionResult> GetUsers()
		{
			var claimsPrincipal = User;
			var response = await _mediator.Send(new GetAllUsersQuery());
			if (!response.Success)
				return StatusCode(500);
			return Ok(response.Users);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetUser(int id)
		{
			var query = new GetUserQuery() { Id = id };
			var response = await _mediator.Send(query);
			if (!response.Success)
			{
				return NotFound(
					new
					{
						errors = response.ErrorMessages
					});
			}

			return Ok(response.User);
		}

		[HttpPost("roles")]
		public async Task<IActionResult> GetRoles()
		{
			var response = await _mediator.Send(new GetAllRolesQuery());
			if (!response.Success)
				return StatusCode(500);
			return Ok(response.Roles);
		}

		[HttpPatch("{id}")]
		public async Task<IActionResult> UpdateUserPatch([FromBody] JsonPatchDocument<User> patchEntity, int id)
		{
			var user = new User();
			patchEntity.ApplyTo(user);

			var validationResult = new UserValidator().Validate(user);
			if (!validationResult.IsValid)
			{
				foreach (var error in validationResult.Errors)
				{
					ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
				}
				return UnprocessableEntity(ModelState);
			}
			var request = new UpdateUserCommand
			{
				AppliedPatch = patchEntity,
				UserId = id
			};

			var response = await _mediator.Send(request);
			if (!response.Success)
				return BadRequest();
			return NoContent();
		}
	}
}
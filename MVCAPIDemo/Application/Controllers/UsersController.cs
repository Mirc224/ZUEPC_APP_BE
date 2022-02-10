using DataAccess.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Application.Commands.Users;
using MVCAPIDemo.Application.Domain;
using MVCAPIDemo.Application.Queries.Users;
using MVCAPIDemo.Application.Validators.Users;
using MVCAPIDemo.Localization;

namespace MVCAPIDemo.Application.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UsersController : ControllerBase
	{

		private readonly IMediator _mediator;

		public UsersController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> GetUsers()
		{
			var response = await _mediator.Send(new GetAllUsersQuery());
			if (!response.Success)
				return StatusCode(500);
			return Ok(response.Users);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetUser(int id)
		{
			var query = new GetUserQuery() { Id = id };
			var result = await _mediator.Send(query);
			if (!result.Success)
			{
				return NotFound(
					new
					{
						errors = result.ErrorMessages
					});
			}

			return Ok(result.User);
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

			return Ok(new { response.Token });
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
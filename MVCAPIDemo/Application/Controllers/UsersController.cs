using DataAccess.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Application.Commands.Users;
using MVCAPIDemo.Application.Queries.Users;
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
        //[Authorize]
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
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand request)
        {
            var response = await _mediator.Send(request);

            return response.Success ? 
                StatusCode(201) : 
                BadRequest(new { error = response.ErrorMessage });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand request)
        {
            var response = await _mediator.Send(request);
            if (!response.Success)
                return Unauthorized(response.ErrorMessage);
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
	}
}
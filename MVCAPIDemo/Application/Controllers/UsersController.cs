using DataAccess.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MVCAPIDemo.Application.Commands.Users;
using MVCAPIDemo.Application.Queries.Users;

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
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var query = new GetUserQuery() { Id = id };
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost("/register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelingApp.Application.Request.Users.Queries;

namespace TravelingApp.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController(IMediator mediator) : Controller
    {

        [HttpGet]
        [Route("getAllUsers")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllUsersQuery request)
        {
            return Ok(await mediator.Send(request));
        }
    }
}

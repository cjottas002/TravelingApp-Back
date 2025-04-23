using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelingApp.Application.Request.Account.Commands.Login;
using TravelingApp.Application.Request.Account.Commands.Register;

namespace TravelingApp.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController(IMediator mediator) : Controller
    {


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand request)
        {
            return Ok(await mediator.Send(request));
        }
    }
}

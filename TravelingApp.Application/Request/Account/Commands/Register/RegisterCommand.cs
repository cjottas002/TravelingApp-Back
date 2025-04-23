using MediatR;
using TravelingApp.Application.Response;
using TravelingApp.Application.Response.Account;

namespace TravelingApp.Application.Request.Account.Commands.Register
{

    public class RegisterCommand : IRequest<FrameworkResponse<RegisterDto>>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}

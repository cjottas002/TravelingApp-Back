using MediatR;
using TravelingApp.Application.Response;
using TravelingApp.Application.Response.Account;

namespace TravelingApp.Application.Request.Account.Commands.Login
{
    public class LoginCommand : IRequest<FrameworkResponse<LoginDto>>
    {
        public string? Username { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
    }


}

using MediatR;
using TravelingApp.Application.Account.Responses.Login;
using TravelingApp.Application.Response;

namespace TravelingApp.Application.Account.Commands.Login
{
    public class LoginCommand : IRequest<FrameworkResponse<LoginResponse>>
    {
        public string? Username { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
    }


}

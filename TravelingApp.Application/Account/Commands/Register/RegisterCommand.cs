using MediatR;
using TravelingApp.Application.Account.Responses.Register;
using TravelingApp.Application.Response;

namespace TravelingApp.Application.Account.Commands.Register
{

    public class RegisterCommand : IRequest<FrameworkResponse<RegisterResponse>>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}

using MediatR;
using TravelingApp.Application.Account.Responses.Login;
using TravelingApp.Application.Common.Interfaces;
using TravelingApp.Application.Response;
using TravelingApp.CrossCutting.Extensions;

namespace TravelingApp.Application.Account.Commands.Login
{
    public class LoginCommandHandler(IAccountService accountService) : IRequestHandler<LoginCommand, FrameworkResponse<LoginResponse>>
    {
        private readonly IAccountService accountService = accountService.ValidateArgument();
        public async Task<FrameworkResponse<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await accountService.LoginAsync(request.Username!, request.Password!);
        }
    }
}

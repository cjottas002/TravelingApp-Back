using MediatR;
using TravelingApp.Application.Common.Interfaces;
using TravelingApp.Application.Response;
using TravelingApp.Application.Response.Account;
using TravelingApp.CrossCutting.Extensions;

namespace TravelingApp.Application.Request.Account.Commands.Login
{
    public class LoginCommandHandler(IAccountService accountService) : IRequestHandler<LoginCommand, FrameworkResponse<LoginDto>>
    {
        private readonly IAccountService accountService = accountService.ValidateArgument();
        public async Task<FrameworkResponse<LoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await accountService.LoginAsync(request.Username!, request.Password!);
        }
    }
}

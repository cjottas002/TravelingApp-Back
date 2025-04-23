using MediatR;
using Microsoft.AspNetCore.Identity;
using TravelingApp.Application.Common.Interfaces;
using TravelingApp.Application.Response;
using TravelingApp.Application.Response.Account;
using TravelingApp.CrossCutting.Extensions;
using TravelingApp.Domain.Entities;

namespace TravelingApp.Application.Request.Account.Commands.Register
{
    public class RegisterCommandHandler(IAccountService accountService) : IRequestHandler<RegisterCommand, FrameworkResponse<RegisterDto>>
    {
        private readonly IAccountService accountService = accountService.ValidateArgument();

        public async Task<FrameworkResponse<RegisterDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await accountService.RegisterAsync(request.Username!, request.Password!);
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Identity;
using TravelingApp.Application.Account.Responses.Register;
using TravelingApp.Application.Common.Interfaces;
using TravelingApp.Application.Response;
using TravelingApp.CrossCutting.Extensions;
using TravelingApp.Domain.Entities;

namespace TravelingApp.Application.Account.Commands.Register
{
    public class RegisterCommandHandler(IAccountService accountService) : IRequestHandler<RegisterCommand, FrameworkResponse<RegisterResponse>>
    {
        private readonly IAccountService accountService = accountService.ValidateArgument();

        public async Task<FrameworkResponse<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await accountService.RegisterAsync(request.Username!, request.Password!);
        }
    }
}

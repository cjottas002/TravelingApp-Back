using MediatR;
using TravelingApp.Application.Common.Interfaces;
using TravelingApp.Application.Response.Users;

namespace TravelingApp.Application.Request.Users.Queries
{
    public class GetAllUsersQueryHandler(IUserService userService) : IRequestHandler<GetAllUsersQuery, UserResponse>
    {

        public async Task<UserResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await userService.GetAllUsersAsync(request);
        }
    }
}

using MediatR;
using TravelingApp.Application.Response.Users;

namespace TravelingApp.Application.Request.Users.Queries
{
    public class GetAllUsersQuery : FrameworkRequest, IRequest<UserResponse> { }
}

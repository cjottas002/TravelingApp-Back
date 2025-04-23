using TravelingApp.Application.Request.Users.Queries;
using TravelingApp.Application.Response.Users;

namespace TravelingApp.Application.Common.Interfaces
{
    public interface IUserService
    {
        public Task<UserResponse> GetAllUsersAsync(GetAllUsersQuery request);
    }
}

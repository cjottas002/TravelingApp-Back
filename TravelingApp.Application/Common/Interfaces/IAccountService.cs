using TravelingApp.Application.Response;
using TravelingApp.Application.Response.Account;

namespace TravelingApp.Application.Common.Interfaces
{
    public interface IAccountService
    {
        Task<FrameworkResponse<LoginDto>> LoginAsync(string email, string password);
        Task<FrameworkResponse<RegisterDto>> RegisterAsync(string email, string password);
    }
}

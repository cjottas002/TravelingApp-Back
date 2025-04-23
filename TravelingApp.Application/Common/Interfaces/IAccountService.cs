using TravelingApp.Application.Account.Responses.Login;
using TravelingApp.Application.Account.Responses.Register;
using TravelingApp.Application.Response;

namespace TravelingApp.Application.Common.Interfaces
{
    public interface IAccountService
    {
        Task<FrameworkResponse<LoginResponse>> LoginAsync(string email, string password);
        Task<FrameworkResponse<RegisterResponse>> RegisterAsync(string email, string password);
    }
}

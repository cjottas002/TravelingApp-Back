using TravelingApp.Application.Response;

namespace TravelingApp.Application.Account.Responses.Login
{
    public class LoginResponse : ResponseDto
    {
        public string? UserId { get; set; }
        public string? Token { get; set; }
    }
}

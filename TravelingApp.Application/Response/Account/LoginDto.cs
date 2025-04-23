namespace TravelingApp.Application.Response.Account
{
    public class LoginDto : ResponseDto
    {
        public string? UserId { get; set; }
        public string? Token { get; set; }
    }
}

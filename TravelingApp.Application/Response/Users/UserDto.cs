namespace TravelingApp.Application.Response.Users
{
    public class UserDto : ResponseDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}

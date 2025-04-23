namespace TravelingApp.Application.Response.Users
{
    public class UserResponse : FrameworkResponse<UserDto>
    {
        public new List<UserDto> Data { get; set; } = [];
    }
}

using AutoMapper;
using TravelingApp.Application.Response.Users;
using TravelingApp.Domain.Entities;
namespace TravelingApp.Application.Mapping
{

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, UserDto>().ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
                                      .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => new DateTimeOffset(src.UpdatedAt).ToUnixTimeMilliseconds()));
        }
    }
}

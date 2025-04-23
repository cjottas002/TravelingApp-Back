using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelingApp.Application.Common.Interfaces;
using TravelingApp.Application.Request.Users.Queries;
using TravelingApp.Application.Response.Users;
using TravelingApp.CrossCutting.Business.Interfaces;
using TravelingApp.CrossCutting.Extensions;
using TravelingApp.Domain.Entities;
using TravelingApp.Infraestructure.Context;

namespace TravelingApp.Infraestructure.Services
{
    public class UserService(IRepository<User, TravelingAppDbContext> repository, IMapper mapper) : IUserService
    {
        private readonly IRepository<User, TravelingAppDbContext> repository = repository.ValidateArgument();
        private readonly IMapper mapper = mapper.ValidateArgument();

        public async Task<UserResponse> GetAllUsersAsync(GetAllUsersQuery request)
        {
            var query = repository.Entity(true);

            var total = await query.CountAsync();

            var paged = query.Page(
                pageSize: request.PageSize,
                pageIndex: request.PageIndex,
                orderBy: request.OrderBy ?? nameof(User.UserName),
                ascending: request.OrderByAsc
            );

            var users = await paged.ToListAsync();
            var dtos = mapper.Map<List<UserDto>>(users);

            return new UserResponse
            {
                Data = dtos,
                Count = total
            };
        }
    }
}
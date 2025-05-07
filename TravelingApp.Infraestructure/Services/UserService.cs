using AutoMapper;
using Microsoft.Extensions.Options;
using TravelingApp.Application.Common.Interfaces;
using TravelingApp.Application.Request.Users.Queries;
using TravelingApp.Application.Response.Users;
using TravelingApp.CrossCutting.Business.Interfaces;
using TravelingApp.CrossCutting.Configuration;
using TravelingApp.CrossCutting.Extensions;
using TravelingApp.Domain.Entities;
using TravelingApp.Infraestructure.Context;
using TravelingApp.Infraestructure.Persistence;

namespace TravelingApp.Infraestructure.Services
{
    public class UserService(
        IRepository<User, TravelingAppDbContext> repository,
        IMapper mapper, 
        IFilterValidationProvider filterValidationProvider,
        ICacheService cacheService,
        IOptions<RedisOptions> opts
        ) : FilteredListObject<User>(filterValidationProvider, cacheService, opts), IUserService
    {
        private readonly IRepository<User, TravelingAppDbContext> repository = repository.ValidateArgument();
        private readonly IMapper mapper = mapper.ValidateArgument();

        public GetAllUsersQuery? Request { get; set; }

        protected override IQueryable<User> CreateQuery()
        {
            return repository!.Entity(true);
        }

        public async Task<UserResponse> GetAllUsersAsync(GetAllUsersQuery request)
        {
            this.Request = request;
            var result = await base.ExecutePagedAsync(Request!.PageSize, Request!.PageIndex, Request.OrderBy!, Request.OrderByAsc);
            if (!result) return new UserResponse();

            var users = this.Results?.ToList();
            var total = this.TotalResults;

            var dtos = mapper.Map<List<UserDto>>(users);

            return new UserResponse
            {
                Data = dtos,
                Count = total
            };
        }
    }
}
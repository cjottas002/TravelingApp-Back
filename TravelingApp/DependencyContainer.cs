using MediatR.Pipeline;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TravelingApp.Domain.Entities;
using TravelingApp.Infraestructure;
using TravelingApp.Infraestructure.Context;
using TravelingApp.Application.Common.Interfaces;
using TravelingApp.Infraestructure.Services;
using TravelingApp.CrossCutting.Configuration;
using TravelingApp.Application.Common.Behaviors;
using FluentValidation;
using TravelingApp.Application.Request.Account.Commands.Login;
using TravelingApp.CrossCutting.Business.Interfaces;
using TravelingApp.Infraestructure.Repositories.TravelingApp.Infrastructure.Repositories;
using TravelingApp.Infraestructure.Persistence;
using TravelingApp.Application.Request.Users.Queries;
using TravelingApp.Infraestructure.Repositories;

namespace TravelingApp
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisDbConnection") ?? "localhost:6379";
                options.InstanceName = $"TravelingApp.Cache:{configuration["ASPNETCORE_ENVIRONMENT"]}:";
            });

            services.Configure<RedisOptions>(configuration.GetSection("RegisOptions"));

            services.AddScoped<ICacheService, RedisCacheService>();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.RegisterDbServices(configuration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = configuration["Jwt:Issuer"],
                     ValidAudience = configuration["Jwt:Audience"],
                     IssuerSigningKey = new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                     ),
                     ClockSkew = TimeSpan.Zero,
                     RoleClaimType = ClaimTypes.Role
                 };

                 options.Events = new JwtBearerEvents
                 {
                     OnMessageReceived = context =>
                     {
                         var token = context.Request.Cookies["AuthToken"];
                         if (!string.IsNullOrEmpty(token))
                         {
                             context.Token = token;
                         }

                         return Task.CompletedTask;
                     }
                 };
             });

            services.AddIdentityCore<User>(opt =>
            {
                opt.SignIn.RequireConfirmedAccount = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
            })
            .AddRoles<IdentityRole>()
            .AddSignInManager<SignInManager<User>>()
            .AddEntityFrameworkStores<TravelingAppDbContext>()
            .AddDefaultTokenProviders();

            services.AddSingleton<Mediator>();
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly);
            });

            services.AddAutoMapper(typeof(GetAllUsersQueryHandler).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(typeof(LoginCommand).Assembly);

            services.AddScoped(typeof(IUnitOfWorkScope<>), typeof(UnitOfWorkScope<>));
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            services.AddScoped<IListFilter, ListFilter>();
            services.AddScoped<IFilterValidationProvider, FilterValidationProvider>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();

        }
    }
}

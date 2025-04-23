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
using TravelingApp.Application.Account.Commands.Login;
using TravelingApp.Application.Common.Interfaces;
using TravelingApp.Infraestructure.Services;
using TravelingApp.CrossCutting.Configuration;
using TravelingApp.Application.Common.Behaviors;
using FluentValidation;

namespace TravelingApp
{
    public static class RegisterServices
    {
        public static void RegisterServicesCore(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<JwtDto>(configuration.GetSection("Jwt"));

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

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly);
            });

            //services.AddAutoMapper(typeof(LoginCommandHandler).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(typeof(LoginCommand).Assembly);



            services.AddScoped<IAccountService, AccountService>();

        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelingApp.Application.Account.Responses.Login;
using TravelingApp.Application.Account.Responses.Register;
using TravelingApp.Application.Common.Interfaces;
using TravelingApp.Application.Response;
using TravelingApp.CrossCutting.Configuration;
using TravelingApp.CrossCutting.Extensions;
using TravelingApp.Domain.Entities;

namespace TravelingApp.Infraestructure.Services
{
    public class AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IOptions<JwtDto> configuration) : IAccountService
    {
        private readonly JwtDto configuration = configuration.Value.ValidateArgument();

        public async Task<FrameworkResponse<LoginResponse>> LoginAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                return new FrameworkResponse<LoginResponse>
                {
                    Errors = [new ValidationResult("Usuario/Contraseña invalida", [username])]
                };
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
            {
                return new FrameworkResponse<LoginResponse>
                {
                    Errors = [new ValidationResult("Usuario/Contraseña invalida", [nameof(password)])]
                };
            }

            var response = new LoginResponse
            {
                UserId = user.Id,
                Token = await this.GenerateJwtTokenAsync(user),
            };

            return new FrameworkResponse<LoginResponse>
            {
                Data = response,
                Count = 1
            };
        }

        public async Task<FrameworkResponse<RegisterResponse>> RegisterAsync(string username, string password)
        {
            var existingUser = await userManager.FindByNameAsync(username);
            if (existingUser != null)
            {
                return new FrameworkResponse<RegisterResponse>
                {
                    Errors = [new ValidationResult("El Usuario ya está registrado", [nameof(username)])]
                };
            }

            var user = new User { UserName = username, Email = username };
            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return new FrameworkResponse<RegisterResponse>
                {
                    Errors = result.Errors.Select(e => new ValidationResult(e.Description, [e.Code]))
                };
            }

            return new FrameworkResponse<RegisterResponse>
            {
                Data = new RegisterResponse()
                {
                    IsRegistered = true
                },
                Count = 1
            };
        }


        private async Task<string> GenerateJwtTokenAsync(User? user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user?.Email ?? ""),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user?.Id.ToString() ?? "")
            };

            var roles = await userManager.GetRolesAsync(user ?? new User());
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration.Issuer,
                audience: configuration.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

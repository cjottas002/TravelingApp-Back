using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using TravelingApp.CrossCutting.AppConstants;
using TravelingApp.Domain.Entities;
using TravelingApp.Infraestructure.Context;

namespace TravelingApp.Infraestructure
{
    public class DataGenerator
    {
        public async static Task Initialize(IServiceCollection services)
        {
            var serviceScopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
            using var scope = serviceScopeFactory?.CreateScope();
            var serviceProvider = scope?.ServiceProvider;

            using var context = serviceProvider?.GetRequiredService<TravelingAppDbContext>();
            if ((context is null) || (context.Users.Any() && context.Roles.Any())) return;

            var roles = new List<IdentityRole>
            {
                new IdentityRole { Id = "47cab13e-ff81-4f9f-b040-7c4994bef9d3", Name = AppConstant.ROL_ADMIN, NormalizedName = AppConstant.ROL_ADMIN.ToUpper() },
                new IdentityRole { Id = "e0d547ff-3c97-4c0d-a03b-d9613509b4af", Name = AppConstant.ROL_CUSTOMER, NormalizedName = AppConstant.ROL_CUSTOMER.ToUpper() },
                new IdentityRole { Id = "820c2aa2-f18d-4eba-b1d6-687537cc0eba", Name = AppConstant.ROL_SUPPLIER, NormalizedName = AppConstant.ROL_SUPPLIER.ToUpper() },
            };

            await context.Roles.AddRangeAsync(roles);

            var users = new List<(string idusuario, string username, string email, string password, string role)>
            {
                ("1234567A", "admin", "admin@test.com", "admin", "admin")
            };

            var userManager = serviceProvider?.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider?.GetRequiredService<RoleManager<IdentityRole>>();


            foreach (var (idusuario, username, email, password, role) in users)
            {
                var user = new User { Id = idusuario, UserName = username, Email = email };

                if (userManager is null) continue;
                try
                {

                    var result = await userManager.CreateAsync(user, password: password);
                    if (result.Succeeded)
                    {
                        if (roleManager is null) continue;
                        var identityRole = await roleManager.FindByNameAsync(role);
                        if (identityRole == null)
                        {
                            identityRole = new IdentityRole(role);
                            await roleManager.CreateAsync(identityRole);
                        }

                        await userManager.AddToRoleAsync(user, role);
                    }
                }
                catch (Exception)
                {

                    throw;
                }


            }

            await context.SaveChangesAsync();
        }
    }
}

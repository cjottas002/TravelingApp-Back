using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelingApp.Infraestructure.Context;

namespace TravelingApp.Infraestructure
{
    public static class RegisterDbService
    {
        public static void RegisterDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TravelingAppDbContext>(options =>
            {
                string travelingConnectionString = configuration.GetConnectionString("TravelingAppDbConnection") ?? string.Empty;
                options.UseSqlServer(travelingConnectionString);
            });
        }
    }
}

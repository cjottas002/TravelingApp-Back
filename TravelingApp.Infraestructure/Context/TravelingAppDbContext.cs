using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TravelingApp.Domain.Entities;

namespace TravelingApp.Infraestructure.Context
{
    public class TravelingAppDbContext(DbContextOptions<TravelingAppDbContext> options, IConfiguration configuration) : IdentityDbContext<User>(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = configuration.GetConnectionString("TravelingAppDbConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("Travel");
            builder.Model.SetDefaultSchema("Travel");

            builder.ApplyConfigurationsFromAssembly(typeof(TravelingAppDbContext).Assembly);
        }


        public virtual DbSet<User> User { get; set; }
    }
}

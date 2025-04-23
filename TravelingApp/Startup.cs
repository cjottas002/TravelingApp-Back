using TravelingApp.CrossCutting.Extensions;
using TravelingApp.Infraestructure;
using TravelingApp.Middleware;

namespace TravelingApp
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.env = env.ValidateArgument();
            this.configuration = configuration.ValidateArgument();

            var builder = new ConfigurationBuilder()
                .SetBasePath(this.env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddUserSecrets<Startup>();

            this.configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterServicesCore(configuration);
            // Inicializa los datos de usuarios y roles
            DataGenerator.Initialize(services).GetAwaiter();
        }

        public void Configure(IApplicationBuilder app)
        {

            app.UseMiddleware<ValidationExceptionMiddleware>();
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else if (env.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = string.Empty;
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                });
            }

            app.UseCors("AllowAllOrigins");

            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}

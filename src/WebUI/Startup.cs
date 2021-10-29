using Application;
using Application.Common.Configuration;
using Autofac;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebUI.Configuration;

namespace WebUI
{
    public class Startup
    {
        private readonly ApplicationConfiguration _applicationConfiguration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _applicationConfiguration = configuration.GetSection(nameof(ApplicationConfiguration))
                .Get<ApplicationConfiguration>();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationConfiguration>(Configuration.GetSection(nameof(ApplicationConfiguration)));
            services.AddApplication(Configuration);
            services.AddDbContext(Configuration);
            services.AddAuthentication(_applicationConfiguration);
            services.AddCustomApiVersioning();
            services.AddControllers();
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // To Do : Add Swagger
            app.UseCustomExceptionHandler();

            app.UseHttpsRedirection();
            app.UseSerilog();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

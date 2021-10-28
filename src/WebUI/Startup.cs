using Application;
using Application.Common.Configuration;
using Autofac;
using Infrastructure.Configurations;
using Infrastructure.Configurations.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebUI.Configuration;

namespace WebUI
{
    /// <summary>
    /// Application StartUp
    /// </summary>
    public class Startup
    {
        private readonly ApplicationConfiguration _applicationConfiguration;

        /// <summary>
        /// StartUp Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _applicationConfiguration = configuration.GetSection(nameof(ApplicationConfiguration))
                .Get<ApplicationConfiguration>();
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Application Services Configuration
        /// </summary>
        /// <param name="services"> IServiceCollection </param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationConfiguration>(Configuration.GetSection(nameof(ApplicationConfiguration)));
            services.AddApplication(Configuration);
            services.AddDbContext(Configuration);
            services.AddAuthentication(_applicationConfiguration);
            services.AddCustomApiVersioning();
            services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            }).AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(namingPolicy: JsonNamingPolicy.CamelCase));
            });
            services.AddSwagger();
        }

        /// <summary>
        /// Autofac IOC
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddServices();
        }

        /// <summary>
        /// Application PipeLine Configuration
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomExceptionHandler();

            app.UseHttpsRedirection();
            app.UseSerilog();

            app.UseRouting();

            app.UseSwaggerWithUi();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using Application.Common.Configuration;
using Autofac.Extensions.DependencyInjection;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace WebUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ConfigureLogging();
            await CreateHost(args);
        }

        private static void ConfigureLogging()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                    optional: true)
                .Build();

            var applicationConfiguration = configuration.GetSection(nameof(ApplicationConfiguration))
                .Get<ApplicationConfiguration>();

            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("Environment", environment)
                .Enrich.WithAssemblyName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName();

            // If environment is development or NeedWriteLogToConsole == true, log to console
            if (environment?.ToLower() == "development" || applicationConfiguration.SerilogConfiguration.NeedWriteLogToConsole)
            {
                loggerConfiguration = loggerConfiguration.WriteTo.Console(LogEventLevel.Information);
            }

            // If NeedWriteLogToFile == true, log to file
            if (applicationConfiguration.SerilogConfiguration.NeedWriteLogToFile)
            {
                loggerConfiguration = loggerConfiguration.WriteTo.File(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "logs", "log.log"),
                    rollingInterval: RollingInterval.Day);
            }

            // If NeedWriteLogToElasticSearch == true, log to elasticSearch
            if (applicationConfiguration.SerilogConfiguration.NeedWriteLogToElasticSearch)
            {
                loggerConfiguration = loggerConfiguration.WriteTo.Elasticsearch(ConfigureElasticSink(environment, applicationConfiguration));
            }

            Log.Logger = loggerConfiguration.ReadFrom.Configuration(configuration).CreateLogger();
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(string environment, ApplicationConfiguration applicationConfiguration)
        {
            return new ElasticsearchSinkOptions(new Uri(applicationConfiguration.SerilogConfiguration.ElasticConfiguration.Uri))
            {
                AutoRegisterTemplate = true,
                CustomFormatter = new ElasticsearchJsonFormatter(),
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }

        private static async Task CreateHost(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<ApplicationDbContext>();
                        if (context.Database.IsSqlServer())
                        {
                            await context.Database.MigrateAsync();
                        }

                        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();


                        await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);
                        await ApplicationDbContextSeed.SeedDefaultProductAndOrderAsync(context,
                            new CancellationToken(false));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("An error occurred while migrating or seeding the database.", ex);
                    }
                }

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal($"Failed to start {Assembly.GetExecutingAssembly().GetName().Name}", ex);
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
    }
}

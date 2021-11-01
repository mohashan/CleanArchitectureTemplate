using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Configurations.Swagger
{
    public static class SwaggerConfigurationExtensions
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            var mainAssembly = Assembly.GetEntryAssembly();
            var mainType = mainAssembly?.GetExportedTypes()[0];
            const string methodName = nameof(Swashbuckle.AspNetCore.Filters.ServiceCollectionExtensions.AddSwaggerExamplesFromAssemblyOf);
            var method = typeof(Swashbuckle.AspNetCore.Filters.ServiceCollectionExtensions).GetRuntimeMethods().FirstOrDefault(x => x.Name == methodName && x.IsGenericMethod);
            var generic = method?.MakeGenericMethod(mainType ?? throw new InvalidOperationException());
            generic?.Invoke(null, new object[] { services });

            services.AddSwaggerGen(options =>
            {
                var xmlDocPath = Path.Combine(AppContext.BaseDirectory, "WebApi.xml");
                options.IncludeXmlComments(xmlDocPath, true);
                options.EnableAnnotations();
                options.DescribeAllParametersInCamelCase();

                options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "API V1" });

                options.ExampleFilters();

                options.OperationFilter<UnauthorizedResponsesOperationFilter>(true, "OAuth2");
                options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("/api/v1/User/SignIn", UriKind.Relative)
                        }
                    }
                });

                options.OperationFilter<RemoveVersionParameters>();
                options.DocumentFilter<SetVersionInPaths>();
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out var methodInfo))
                    {
                        return false;
                    }

                    var versions = (methodInfo.DeclaringType ?? throw new InvalidOperationException())
                        .GetCustomAttributes<ApiVersionAttribute>(true)
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v}" == docName);
                });
            });
        }

        public static IApplicationBuilder UseSwaggerWithUi(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
                options.DocExpansion(DocExpansion.None);
                options.ShowExtensions();
            });
            return app;
        }
    }
}
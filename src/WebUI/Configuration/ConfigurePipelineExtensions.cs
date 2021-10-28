using Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;

namespace WebUI.Configuration
{
    public static class ConfigurePipelineExtensions
    {
        public static void UseSerilog(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging(options =>
            {
                // Customize the message template
                options.MessageTemplate = "Handled {RequestPath}";

                // Emit debug-level events instead of the defaults
                options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

                // Attach additional properties to the request completion event
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                    diagnosticContext.Set("RequestHeader", httpContext.Request.Headers);
                    diagnosticContext.Set("RequestBody", httpContext.Request.Body);
                    diagnosticContext.Set("RequestQueryString", httpContext.Request.QueryString);
                };
            });
        }

        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }

    }
}
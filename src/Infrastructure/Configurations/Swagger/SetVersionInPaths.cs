using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infrastructure.Configurations.Swagger
{
    public class SetVersionInPaths : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var updatedPaths = new OpenApiPaths();

            foreach (var (key, value) in swaggerDoc.Paths)
            {
                updatedPaths.Add(
                    key.Replace("v{version}", swaggerDoc.Info.Version),
                    value);
            }

            swaggerDoc.Paths = updatedPaths;
        }
    }
}
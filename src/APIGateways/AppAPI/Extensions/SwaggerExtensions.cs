using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AppAPI.Extensions;

public static class SwaggerExtensions
{
    internal static void AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.ResolveConflictingActions (apiDescriptions => apiDescriptions.First ());
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "CMS App API", Version = "v1"});
            // c.OperationFilter<AddRequiredHeaderParameter>();
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new string[] { }
                }
            });
        });
    }

    private class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "LanguageCode",
                In = ParameterLocation.Header,
            });
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "DeviceId",
                In = ParameterLocation.Header,
            });
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "DeviceToken",
                In = ParameterLocation.Header,
            });
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "OsType",
                In = ParameterLocation.Header,
            });
        }
    }
}

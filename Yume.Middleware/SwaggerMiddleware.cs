using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Yume.Middleware
{
    public static class SwaggerMiddleware
    {
        public static void AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("V1", new OpenApiInfo() { Title = "API V1", Version = "V1.0", Description = "Version 1" });

                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                // For V2-V1 complications with schemeID's
                options.CustomSchemaIds(x => x.FullName);

                // Define the security scheme and Add the security scheme
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                });

                // Make sure Swagger UI requires a Bearer token to be included
                // Add the security requirement
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public static void UseCustomSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/V1/swagger.json", "V1.0");

                // Add the authorization header input box in Swagger UI
                //options.DefaultModelsExpandDepth(-1);
                //options.DisplayRequestDuration();
                //options.EnableDeepLinking();
                //options.EnableValidator();
                //options.ShowExtensions();
                //options.EnableFilter();
                //options.ShowCommonExtensions();
                //options.InjectStylesheet("/swagger/custom.css"); // customize the appearance
            });
        }
    }
}

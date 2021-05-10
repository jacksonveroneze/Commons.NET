using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace JacksonVeroneze.NET.Commons.Swagger
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services,
            Action<SwaggerOptions> action)
        {
            SwaggerOptions optionsConfig = new SwaggerOptions();

            action.Invoke(optionsConfig);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(optionsConfig.Version, new OpenApiInfo
                    {
                        Title = optionsConfig.Title,
                        Version = optionsConfig.Version,
                        Description = optionsConfig.Description,
                        Contact = new OpenApiContact
                            {Name = optionsConfig.ContactName, Email = optionsConfig.ContactEmail},
                        License = new OpenApiLicense
                            {Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT")}
                    }
                );

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Input the JWT like: Bearer {your token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                        new string[] { }
                    }
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app,
            IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = String.Empty;

                foreach (var description in provider.ApiVersionDescriptions)
                    c.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
            });

            return app;
        }
    }
}
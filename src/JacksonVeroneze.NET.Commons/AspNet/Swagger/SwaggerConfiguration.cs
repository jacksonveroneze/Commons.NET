using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace JacksonVeroneze.NET.Commons.AspNet.Swagger
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services,
            Action<SwaggerOptions> action)
        {
            SwaggerOptions optionsConfig = new SwaggerOptions();

            action?.Invoke(optionsConfig);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(optionsConfig.Version, new OpenApiInfo
                    {
                        Title = optionsConfig.Title,
                        Version = optionsConfig.Version,
                        Description = optionsConfig.Description,
                        Contact = new OpenApiContact
                        {
                            Name = optionsConfig.ContactName,
                            Email = optionsConfig.ContactEmail
                        },
                        License = new OpenApiLicense
                        {
                            Name = "MIT",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    }
                );

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Input the JWT like: Bearer {your token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });

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
                        new string[] { }
                    }
                });

                if (optionsConfig.IncludeXmlComments)
                    options.IncludeXmlComments(optionsConfig.XmlCommentsPath);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app,
            IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = String.Empty;

                foreach (var description in provider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
            });

            return app;
        }
    }
}
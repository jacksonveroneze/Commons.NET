using System;
using System.IO;
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

            ServiceProvider provider = services.BuildServiceProvider();

            IApiVersionDescriptionProvider descriptionProvider =
                provider.GetRequiredService<IApiVersionDescriptionProvider>();

            services.AddSwaggerGen(options =>
            {
                foreach (ApiVersionDescription description in descriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo
                        {
                            Title = optionsConfig.Title,
                            Version = description.ApiVersion.ToString(),
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
                }

                if (optionsConfig.UseAuthentication)
                {
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
                }

                if (!string.IsNullOrEmpty(optionsConfig.AssemblyName))
                {
                    string xmlFile = $"{optionsConfig.AssemblyName}.xml";

                    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                    if (File.Exists(xmlFile))
                        options.IncludeXmlComments(xmlPath);
                }
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

                foreach (ApiVersionDescription versionDescription in provider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/swagger/{versionDescription.GroupName}/swagger.json",
                        versionDescription.GroupName.ToUpperInvariant());
            });

            return app;
        }
    }
}
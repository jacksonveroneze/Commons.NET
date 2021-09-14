using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.NET.Commons.AspNet.ApiVersioning
{
    public static class ApiVersioningConfiguration
    {
        public static IServiceCollection AddApiVersioningConfiguration(this IServiceCollection services,
            Action<ApiVersioningOptions> action)
        {
            ApiVersioningOptions optionsConfig = new ApiVersioningOptions();

            action?.Invoke(optionsConfig);

            services.AddApiVersioning(p =>
            {
                p.DefaultApiVersion = new ApiVersion(optionsConfig.MajorVersion, optionsConfig.MinorVersion);
                p.ReportApiVersions = true;
                p.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddVersionedApiExplorer(p =>
            {
                p.GroupNameFormat = "'v'VVV";
                p.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}
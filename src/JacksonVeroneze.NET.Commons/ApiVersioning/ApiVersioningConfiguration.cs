using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.NET.Commons.ApiVersioning
{
    public static class ApiVersioningConfiguration
    {
        public static IServiceCollection AddApiVersioningConfiguration(this IServiceCollection services,
            Action<ApiVersioningOptions> action)
        {
            ApiVersioningOptions apiVersioningOptions = new ApiVersioningOptions();

            action.Invoke(apiVersioningOptions);

            services.AddApiVersioning(p =>
            {
                p.DefaultApiVersion =
                    new ApiVersion(apiVersioningOptions.MajorVersion, apiVersioningOptions.MinorVersion);
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
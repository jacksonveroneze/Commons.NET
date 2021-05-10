using System;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.NET.Commons.Cors
{
    public static class CorsConfiguration
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services,
            Action<CorsOptions> action)
        {
            CorsOptions optionsConfig = new CorsOptions();

            action.Invoke(optionsConfig);

            return services.AddCors(options =>
            {
                options.AddPolicy(optionsConfig.Policy,
                    builder =>
                    {
                        builder
                            .WithOrigins(optionsConfig.UrlsAllowed)
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
        }
    }
}
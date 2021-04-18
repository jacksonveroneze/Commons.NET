using System;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.NET.Commons.Cors
{
    public static class CorsConfiguration
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services,
            Action<CorsOptions> action)
        {
            CorsOptions corsOptions = new CorsOptions();

            action.Invoke(corsOptions);

            return services.AddCors(options =>
            {
                options.AddPolicy(corsOptions.Policy,
                    builder =>
                    {
                        builder
                            .WithOrigins(corsOptions.UrlsAllowed)
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
        }
    }
}
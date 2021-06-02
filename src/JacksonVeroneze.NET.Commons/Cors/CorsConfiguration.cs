using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.NET.Commons.Cors
{
    public static class CorsConfiguration
    {
        private static string _policy = "CorsPolicy";

        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services,
            Action<CorsOptions> action)
        {
            CorsOptions optionsConfig = new CorsOptions();

            action.Invoke(optionsConfig);

            return services.AddCors(options =>
            {
                options.AddPolicy(_policy,
                    builder =>
                    {
                        builder
                            .WithOrigins(optionsConfig.UrlsAllowed)
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
        }

        public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app)
        {
            app.UseCors(_policy);

            return app;
        }
    }
}
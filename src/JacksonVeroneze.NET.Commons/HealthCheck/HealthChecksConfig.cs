using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.NET.Commons.HealthCheck
{
    public static class HealthCheckConfiguration
    {
        public static IServiceCollection AddHealthCheckConfiguration(this IServiceCollection services)
        {
            services.AddHealthChecks();

            return services;
        }

        public static IApplicationBuilder UseHealthCheckConfiguration(this IApplicationBuilder app)
            => app.UseHealthChecks("/health");
    }
}
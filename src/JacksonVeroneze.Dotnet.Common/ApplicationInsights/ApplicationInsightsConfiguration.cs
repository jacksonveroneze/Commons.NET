using System;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.Dotnet.Common.ApplicationInsights
{
    public static class ApplicationInsightsConfiguration
    {
        public static IServiceCollection AddApplicationInsightsConfiguration(this IServiceCollection services,
            Action<ApplicationInsightsOptions> action)
        {
            ApplicationInsightsOptions applicationInsightsOptions = new ApplicationInsightsOptions();

            action.Invoke(applicationInsightsOptions);

            if (string.IsNullOrEmpty(applicationInsightsOptions.InstrumentationKey) is false)
                services.AddApplicationInsightsTelemetry(applicationInsightsOptions.InstrumentationKey);

            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) =>
            {
                module.EnableSqlCommandTextInstrumentation = true;
                module.EnableRequestIdHeaderInjectionInW3CMode = true;
            });

            return services;
        }
    }
}
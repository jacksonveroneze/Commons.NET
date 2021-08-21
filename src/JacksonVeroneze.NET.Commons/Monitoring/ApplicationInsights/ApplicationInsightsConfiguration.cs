using System;
using System.Diagnostics;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.NET.Commons.Monitoring.ApplicationInsights
{
    public static class ApplicationInsightsConfiguration
    {
        public static IServiceCollection AddApplicationInsightsConfiguration(this IServiceCollection services,
            Action<ApplicationInsightsOptions> action)
        {
            ApplicationInsightsOptions optionsConfig = new ApplicationInsightsOptions();

            action?.Invoke(optionsConfig);

            if (string.IsNullOrEmpty(optionsConfig.InstrumentationKey) is false)
                services.AddApplicationInsightsTelemetry(optionsConfig.InstrumentationKey);

            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, _) =>
                module.EnableSqlCommandTextInstrumentation = true);

            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            return services;
        }
    }
}
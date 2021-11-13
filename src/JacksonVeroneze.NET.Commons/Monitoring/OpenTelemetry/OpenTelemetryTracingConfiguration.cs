using System;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace JacksonVeroneze.NET.Commons.Monitoring.OpenTelemetry
{
    public static class OpenTelemetryTracingConfiguration
    {
        public static IServiceCollection AddOpenTelemetryTracingConfiguration(this IServiceCollection services,
            Action<OpenTelemetryTracingOptions> action)
        {
            OpenTelemetryTracingOptions optionsConfig = new OpenTelemetryTracingOptions();

            action?.Invoke(optionsConfig);

            return services.AddOpenTelemetryTracing(
                builder =>
                {
                    builder
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(optionsConfig.ApplicationName))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddSqlClientInstrumentation(options => options.SetDbStatementForText = false);

                    if (optionsConfig.UseJaeger)
                        builder.AddJaegerExporter(options =>
                        {
                            options.AgentHost = optionsConfig.JaegerAgentHost;
                            options.AgentPort = optionsConfig.JaegerAgentPort;
                        });

                    if (optionsConfig.ShowConsoleExporter)
                        builder.AddConsoleExporter();

                    if (optionsConfig.UseGrafanaAgent)
                        builder.AddOtlpExporter(opt =>
                        {
                            opt.Endpoint = new Uri(optionsConfig.GrafanaAgentHost);
                        });
                });
        }
    }
}
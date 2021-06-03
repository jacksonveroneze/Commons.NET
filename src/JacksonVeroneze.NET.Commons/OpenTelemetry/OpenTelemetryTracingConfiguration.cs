using System;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace JacksonVeroneze.NET.Commons.OpenTelemetry
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
                        .AddSqlClientInstrumentation(options => { options.SetTextCommandContent = true; })
                        .AddJaegerExporter(options =>
                        {
                            options.AgentHost = optionsConfig.JaegerAgentHost;
                            options.AgentPort = optionsConfig.JaegerAgentPort;
                        });

                    if (optionsConfig.ShowConsoleExporter)
                        builder.AddConsoleExporter();
                });
        }
    }
}
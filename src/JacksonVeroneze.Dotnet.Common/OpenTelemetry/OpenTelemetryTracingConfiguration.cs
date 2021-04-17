using System;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace JacksonVeroneze.Dotnet.Common.OpenTelemetry
{
    public static class OpenTelemetryTracingConfiguration
    {
        public static IServiceCollection AddOpenTelemetryTracingConfiguration(this IServiceCollection services,
            Action<OpenTelemetryTracingOptions> action)
        {
            OpenTelemetryTracingOptions optionsCfg = new OpenTelemetryTracingOptions();

            action.Invoke(optionsCfg);

            return services.AddOpenTelemetryTracing(
                builder => builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(optionsCfg.ApplicationName))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation(options => { options.SetTextCommandContent = true; })
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = optionsCfg.JaegerAgentHost;
                        options.AgentPort = optionsCfg.JaegerAgentPort;
                    })
                    .AddConsoleExporter());
        }
    }
}
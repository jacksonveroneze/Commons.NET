using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace JacksonVeroneze.NET.Commons.Logger
{
    public static class Logger
    {
        public static ILogger FactoryLogger(Action<LoggerOptions> action)
        {
            LoggerOptions optionsCfg = new LoggerOptions();

            action.Invoke(optionsCfg);

            IConfigurationRoot configuration = FactoryConfiguration(optionsCfg);

            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("ApplicationName", optionsCfg.ApplicationName)
                .Enrich.WithProperty("Environment", optionsCfg.Environment)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithDemystifiedStackTraces()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}{NewLine}{Message:lj} {Properties:j}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate);

            if (string.IsNullOrEmpty(configuration["ApplicationInsights_InstrumentationKey"]) is false)
                loggerConfiguration.WriteTo.ApplicationInsights(configuration["ApplicationInsights_InstrumentationKey"],
                    TelemetryConverter.Events);

            return loggerConfiguration.CreateLogger();
        }

        private static IConfigurationRoot FactoryConfiguration(LoggerOptions optionsCfg)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());

            bool isDevelopment = IsDevelopmentEnvironment(optionsCfg);

            if (isDevelopment && File.Exists(Path.Combine(optionsCfg.CurrentDirectory, "appsettings.json")))
                builder.AddJsonFile("appsettings.json", true, true);

            if (isDevelopment &&
                File.Exists(Path.Combine(optionsCfg.CurrentDirectory, "appsettings.Development.json")))
                builder.AddJsonFile("appsettings.Development.json", true, true);

            return builder
                .AddEnvironmentVariables("APP_CONFIG_")
                .Build();
        }

        private static bool IsDevelopmentEnvironment(LoggerOptions optionsCfg)
        {
            string environment = optionsCfg.Environment;

            return environment != null &&
                   environment.Equals("Development", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
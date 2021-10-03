using JacksonVeroneze.NET.Commons.AspNet.Culture;
using JacksonVeroneze.NET.Commons.AspNet.HealthCheck;
using JacksonVeroneze.NET.Commons.AspNet.Middlewares.ErrorHandling;
using JacksonVeroneze.NET.Commons.AspNet.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JacksonVeroneze.NET.Commons.AspNet
{
    public abstract class BaseStartup
    {
        protected IConfiguration Configuration { get; }

        protected IHostEnvironment HostEnvironment { get; }

        protected IConfigurationBuilder ConfigurationBuilder { get; } = new ConfigurationBuilder();

        protected BaseStartup(IHostEnvironment hostEnvironment)
        {
            HostEnvironment = hostEnvironment;

            ConfigurationBuilder.SetBasePath(hostEnvironment.ContentRootPath);

            if (!hostEnvironment.IsProduction())
                ConfigurationBuilder.AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true);

            ConfigurationBuilder.AddEnvironmentVariables("APP_CONFIG_");
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddRoutingConfiguration()
                .AddHealthCheckConfiguration();
        }

        public virtual void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseCultureConfiguration()
                .UseMiddleware<ErrorHandlingMiddleware>()
                .UseRouting()
                .UseHealthCheckConfiguration();
        }
    }
}
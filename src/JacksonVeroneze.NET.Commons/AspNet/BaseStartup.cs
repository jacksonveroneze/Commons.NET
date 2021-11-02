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
        protected IHostEnvironment HostEnvironment { get; set; }

        protected IConfiguration Configuration { get; set; }

        protected BaseStartup(IHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            HostEnvironment = hostEnvironment;
            Configuration = configuration;
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
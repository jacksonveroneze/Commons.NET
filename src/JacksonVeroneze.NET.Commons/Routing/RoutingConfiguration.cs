using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.NET.Commons.Routing
{
    public static class RoutingConfiguration
    {
        public static IServiceCollection AddRoutingConfiguration(this IServiceCollection services)
            => services.AddRouting(options => options.LowercaseUrls = true);
    }
}
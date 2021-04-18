using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.Dotnet.Commons.Routing
{
    public static class RoutingConfiguration
    {
        public static IServiceCollection AddRoutingConfiguration(this IServiceCollection services)
            => services.AddRouting(options => options.LowercaseUrls = true);
    }
}
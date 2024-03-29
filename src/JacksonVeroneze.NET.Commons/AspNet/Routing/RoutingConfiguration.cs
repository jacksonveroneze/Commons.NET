using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.NET.Commons.AspNet.Routing
{
    public static class RoutingConfiguration
    {
        public static IServiceCollection AddRoutingConfiguration(this IServiceCollection services)
            => services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
    }
}
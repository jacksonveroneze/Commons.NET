using Microsoft.AspNetCore.Builder;

namespace JacksonVeroneze.Dotnet.Common.Middlewares
{
    public static class MaxRequestBodyLimitBootstrapper
    {
        public static IApplicationBuilder UseMaxRequestBodyLimit(this IApplicationBuilder builder, long maxLimitSize)
            => builder.UseMiddleware<MaxRequestBodyLimitMiddleware>();
    }
}
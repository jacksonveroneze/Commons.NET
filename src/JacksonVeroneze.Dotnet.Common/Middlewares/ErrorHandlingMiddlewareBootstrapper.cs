using Microsoft.AspNetCore.Builder;

namespace JacksonVeroneze.Dotnet.Common.Middlewares
{
    public static class ErrorHandlingMiddlewareBootstrapper
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}

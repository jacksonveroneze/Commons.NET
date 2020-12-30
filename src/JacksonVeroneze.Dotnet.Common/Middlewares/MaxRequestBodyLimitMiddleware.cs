using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace JacksonVeroneze.Dotnet.Common.Middlewares
{
    public class MaxRequestBodyLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly long _maxLimitSize;

        public MaxRequestBodyLimitMiddleware(RequestDelegate next, long maxLimitSize = 100)
        {
            _next = next;
            _maxLimitSize = maxLimitSize;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            IHttpMaxRequestBodySizeFeature maxRequestBodySizeFeature =
                httpContext.Features.Get<IHttpMaxRequestBodySizeFeature>();

            if (maxRequestBodySizeFeature != null && maxRequestBodySizeFeature.IsReadOnly) maxRequestBodySizeFeature.MaxRequestBodySize = _maxLimitSize;

            await _next(httpContext);
        }
    }
}
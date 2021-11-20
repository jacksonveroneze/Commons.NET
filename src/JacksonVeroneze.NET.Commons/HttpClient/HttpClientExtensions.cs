using CorrelationId.HttpClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace JacksonVeroneze.NET.Commons.HttpClient
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddHttpClient<TClient, TClientOptions>(this IServiceCollection services,
            IConfiguration configuration, string configurationSectionName)
            where TClient : class
            where TClientOptions : HttpClientOptions, new() =>
            services
                .Configure<TClientOptions>(configuration.GetSection(configurationSectionName))
                .AddRefitClient<TClient>()
                .ConfigureHttpClient(
                    (serviceProvider, httpClient) =>
                    {
                        var httpClientOptions = serviceProvider
                            .GetRequiredService<IOptions<TClientOptions>>()
                            .Value;
                        httpClient.BaseAddress = httpClientOptions.BaseAddress;
                        httpClient.Timeout = httpClientOptions.Timeout;
                    })
                .AddCorrelationIdForwarding()
                .AddHttpMessageHandler<AuthorizationHeaderHandler>()
                .AddPolicyHandlerFromRegistry(PolicyName.HttpRetry)
                .AddPolicyHandlerFromRegistry(PolicyName.HttpCircuitBreaker)
                .Services;
    }
}

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;

namespace JacksonVeroneze.NET.Commons.HttpClient
{
    public static class PolicyPolicies
    {
        private const string PoliciesConfigurationSectionName = "Policies";

        public static IServiceCollection AddPolicies(this IServiceCollection services, IConfiguration configuration,
            string configurationSectionName = PoliciesConfigurationSectionName)
        {
            services.Configure<PolicyOptions>(configuration);

            PolicyOptions policyOptions = configuration.GetSection(configurationSectionName).Get<PolicyOptions>();

            IPolicyRegistry<string> policyRegistry = services.AddPolicyRegistry();

            policyRegistry.Add(
                PolicyName.HttpRetry,
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        policyOptions.HttpRetry.Count,
                        retryAttempt =>
                            TimeSpan.FromSeconds(Math.Pow(policyOptions.HttpRetry.BackoffPower, retryAttempt))));

            policyRegistry.Add(
                PolicyName.HttpCircuitBreaker,
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .CircuitBreakerAsync(
                        policyOptions.HttpCircuitBreaker.ExceptionsAllowedBeforeBreaking,
                        policyOptions.HttpCircuitBreaker.DurationOfBreak));

            return services;
        }
    }
}

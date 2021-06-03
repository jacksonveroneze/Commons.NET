using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.NET.Commons.Authorization
{
    public static class AuthorizationConfiguration
    {
        public static IServiceCollection AddAuthorizationConfiguration(this IServiceCollection services,
            Action<AuthorizationOptionss> action)
        {
            AuthorizationOptionss optionsConfig = new AuthorizationOptionss();

            action?.Invoke(optionsConfig);

            services.AddAuthorization(options =>
            {
                foreach (string customPolice in optionsConfig.Polices)
                    options.AddCustomPolicy(customPolice, optionsConfig.Authority);
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }

        private static void AddCustomPolicy(this AuthorizationOptions options, string policyName,
            string authority)
        {
            options.AddPolicy(policyName,
                policy => policy.Requirements.Add(new HasScopeRequirement(policyName, authority)));
        }
    }
}
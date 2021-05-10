using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.NET.Commons.Authentication
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services,
            Action<AuthenticationOptions> action)
        {
            AuthenticationOptions optionsConfig = new AuthenticationOptions();

            action.Invoke(optionsConfig);

            if (string.IsNullOrEmpty(optionsConfig.Audience) ||
                string.IsNullOrEmpty(optionsConfig.Authority))
                throw new ArgumentException("Configuração do JWT não definida corretamente.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = optionsConfig.Authority;
                options.Audience = optionsConfig.Audience;
            });

            return services;
        }
    }
}
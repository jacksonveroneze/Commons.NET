using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.Dotnet.Common.Authentication
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services,
            Action<AuthenticationOptions> action)
        {
            AuthenticationOptions authenticationOptions = new AuthenticationOptions();

            action.Invoke(authenticationOptions);

            if (string.IsNullOrEmpty(authenticationOptions.Audience) ||
                string.IsNullOrEmpty(authenticationOptions.Authority))
                throw new ArgumentException("Configuração do JWT não definida corretamente.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = authenticationOptions.Authority;
                options.Audience = authenticationOptions.Audience;
            });

            return services;
        }
    }
}
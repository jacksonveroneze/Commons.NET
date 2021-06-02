using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.NET.Commons.Database
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddDatabaseConfiguration<T>(this IServiceCollection services,
            Action<DatabaseOptions> action) where T: DbContext
        {
            DatabaseOptions optionsConfig = new DatabaseOptions();

            action.Invoke(optionsConfig);

            return services.AddDbContext<T>((_, options) =>
                options
                    .UseSqlServer(optionsConfig.ConnectionString,
                        optionsBuilder =>
                        {
                            optionsBuilder
                                .CommandTimeout((int) TimeSpan.FromMinutes(3).TotalSeconds)
                                .EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                        })
                    .UseLazyLoadingProxies()
                    .UseSnakeCaseNamingConvention()
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging());
        }
    }
}
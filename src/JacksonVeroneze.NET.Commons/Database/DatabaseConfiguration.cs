using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JacksonVeroneze.NET.Commons.Database
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddSqlServerDatabaseConfiguration<T>(this IServiceCollection services,
            Action<DatabaseOptions> action) where T : DbContext
        {
            DatabaseOptions optionsConfig = FactoryOptions(action);

            return services.AddDbContext<T>((_, options) =>
                options.UseSqlServer(optionsConfig.ConnectionString, optionsBuilder =>
                        optionsBuilder
                            .CommandTimeout((int) TimeSpan.FromMinutes(3).TotalSeconds)
                            .EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
                    )
                    .ConfigureOptions(optionsConfig)
                    .ConfigureLogger(optionsConfig)
            );
        }

        public static IServiceCollection AddPostgreSqlDatabaseConfiguration<T>(this IServiceCollection services,
            Action<DatabaseOptions> action) where T : DbContext
        {
            DatabaseOptions optionsConfig = FactoryOptions(action);

            return services.AddDbContext<T>((_, options) =>
                options.UseNpgsql(optionsConfig.ConnectionString, optionsBuilder =>
                        optionsBuilder
                            .CommandTimeout((int) TimeSpan.FromMinutes(3).TotalSeconds)
                            .EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
                    )
                    .ConfigureOptions(optionsConfig)
                    .ConfigureLogger(optionsConfig)
            );
        }

        public static IServiceCollection AddSqliteDatabaseConfiguration<T>(this IServiceCollection services,
            Action<DatabaseOptions> action) where T : DbContext
        {
            DatabaseOptions optionsConfig = FactoryOptions(action);

            return services.AddDbContext<T>((_, options) =>
                options.UseSqlite(optionsConfig.ConnectionString, optionsBuilder =>
                        optionsBuilder
                            .CommandTimeout((int) TimeSpan.FromMinutes(3).TotalSeconds)
                    )
                    .ConfigureOptions(optionsConfig)
                    .ConfigureLogger(optionsConfig)
            );
        }

        private static DatabaseOptions FactoryOptions(Action<DatabaseOptions> action)
        {
            DatabaseOptions optionsConfig = new DatabaseOptions();

            action?.Invoke(optionsConfig);

            return optionsConfig;
        }

        private static DbContextOptionsBuilder ConfigureOptions(this DbContextOptionsBuilder options,
            DatabaseOptions optionsConfig)
        {
            options.UseSnakeCaseNamingConvention();

            if (optionsConfig.UseLazyLoadingProxies)
                options.UseLazyLoadingProxies();

            if (optionsConfig.EnableDetailedErrors)
                options.EnableDetailedErrors();

            if (optionsConfig.EnableSensitiveDataLogging)
                options.EnableSensitiveDataLogging();

            return options;
        }

        private static DbContextOptionsBuilder ConfigureLogger(this DbContextOptionsBuilder options,
            DatabaseOptions optionsConfig)
            => options;
    }
}
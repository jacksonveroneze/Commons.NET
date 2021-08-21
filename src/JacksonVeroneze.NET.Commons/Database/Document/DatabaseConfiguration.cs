using System;
using JacksonVeroneze.NET.Commons.Data.Document;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace JacksonVeroneze.NET.Commons.Database.Document
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddMongoDbConfiguration(this IServiceCollection services,
            Action<DatabaseOptions> action)
        {
            DatabaseOptions optionsConfig = new DatabaseOptions();

            action?.Invoke(optionsConfig);

            services.AddScoped<IMongoContext>(x =>
                new MongoContext(new MongoClient(optionsConfig.ConnectionString), optionsConfig.DatabaseName));

            return services;
        }
    }
}
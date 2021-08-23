using System;
using JacksonVeroneze.NET.Commons.Data.Document;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace JacksonVeroneze.NET.Commons.Database.Document
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddMongoDbConfiguration(this IServiceCollection services,
            Action<DatabaseOptions> action)
        {
            DatabaseOptions optionsConfig = new DatabaseOptions();

            action?.Invoke(optionsConfig);

            MongoUrl mongoConnectionUrl = new MongoUrl(optionsConfig.ConnectionString);
            MongoClientSettings mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);

            mongoClientSettings.ClusterConfigurator = cb =>
            {
                cb.Subscribe<CommandStartedEvent>(e =>
                {
                    optionsConfig.Logger.LogInformation($"{e.CommandName} - {e.Command.ToJson()}");
                });

                cb.Subscribe<CommandSucceededEvent>(e =>
                {
                    optionsConfig.Logger.LogInformation($"{e.CommandName} - {e.ToJson()}");
                });
                
                cb.Subscribe<CommandFailedEvent>(e =>
                {
                    optionsConfig.Logger.LogError($"{e.CommandName} - {e.ToJson()}");
                });
            };

            services.AddScoped<IMongoContext>(x =>
                new MongoContext(new MongoClient(mongoClientSettings), optionsConfig.DatabaseName));

            return services;
        }
    }
}
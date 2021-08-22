using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace JacksonVeroneze.NET.Commons.Data.Document
{
    public class MongoContext : IMongoContext
    {
        private IMongoDatabase Database { get; set; }

        public MongoClient MongoClient { get; set; }

        private readonly List<Func<Task>> _commands;

        public IClientSessionHandle Session { get; set; }

        public MongoContext(MongoClient mongoClient, string databaseName)
        {
            _commands = new List<Func<Task>>();

            RegisterConventions();

            MongoClient = mongoClient;

            Database = MongoClient.GetDatabase(databaseName);
        }

        private void RegisterConventions()
        {
            ConventionPack pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfDefaultConvention(true),
            };

            ConventionRegistry.Register("Conventions", pack, t => true);
        }

        public async Task<int> SaveChanges()
        {
            IEnumerable<Task> commandTasks = _commands.Select(c => c());

            await Task.WhenAll(commandTasks);

            return _commands.Count;
        }

        public IMongoCollection<T> GetCollection<T>(string name)
            => Database.GetCollection<T>(name);

        public void Dispose()
            => GC.SuppressFinalize(this);

        public Task AddCommand(Func<Task> func)
        {
            _commands.Add(func);

            return Task.CompletedTask;
        }
    }
}
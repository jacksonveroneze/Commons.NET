using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace JacksonVeroneze.NET.Commons.Database.Data.Document
{
    public interface IMongoContext
    {
        Task AddCommand(Func<Task> func);

        Task<int> SaveChanges();

        IMongoCollection<T> GetCollection<T>(string name);

        MongoClient Client { get; }
    }
}
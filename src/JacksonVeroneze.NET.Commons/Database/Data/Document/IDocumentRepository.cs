using JacksonVeroneze.NET.Commons.DomainObjects;

namespace JacksonVeroneze.NET.Commons.Database.Data.Document
{
    public interface IDocumentRepository<TEntity, in TId> : IRepository<TEntity, TId>
        where TEntity : IAggregateRoot where TId : EntityId
    {
    }
}
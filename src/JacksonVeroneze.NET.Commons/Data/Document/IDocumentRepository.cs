using JacksonVeroneze.NET.Commons.DomainObjects;

namespace JacksonVeroneze.NET.Commons.Data.Document
{
    public interface IDocumentRepository<TEntity, in TId> : IRepository<TEntity, TId>
        where TEntity : EntityRoot where TId : EntityId
    {
    }
}
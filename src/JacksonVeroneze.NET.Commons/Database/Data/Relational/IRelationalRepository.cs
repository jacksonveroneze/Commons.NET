using JacksonVeroneze.NET.Commons.DomainObjects;

namespace JacksonVeroneze.NET.Commons.Database.Data.Relational
{
    public interface IRelationalRepository<TEntity, in TId> : IRepository<TEntity, TId>
        where TEntity : IAggregateRoot where TId : EntityId
    {
    }
}
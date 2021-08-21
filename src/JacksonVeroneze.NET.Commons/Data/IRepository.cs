using System.Collections.Generic;
using System.Threading.Tasks;
using JacksonVeroneze.NET.Commons.DomainObjects;

namespace JacksonVeroneze.NET.Commons.Data
{
    public interface IRepository<TEntity, in TId> where TEntity : EntityRoot where TId : EntityId
    {
        public IUnitOfWork UnitOfWork { get; set; }

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        ValueTask<TEntity> FindAsync(TId simpleId);

        Task<TEntity> FindAsync<TFilter>(TFilter filter) where TFilter : BaseFilter<TEntity>;

        Task<List<TEntity>> FilterAsync<TFilter>(TFilter filter) where TFilter : BaseFilter<TEntity>;

        Task<List<TEntity>> FilterAsync<TFilter>(Pagination pagination, TFilter filter)
            where TFilter : BaseFilter<TEntity>;

        Task<Pageable<TEntity>> FilterPaginateAsync<TFilter>(Pagination pagination, TFilter filter)
            where TFilter : BaseFilter<TEntity>;

        Task<int> CountAsync<TFilter>(TFilter filter) where TFilter : BaseFilter<TEntity>;
    }
}
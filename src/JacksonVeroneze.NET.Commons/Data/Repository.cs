using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JacksonVeroneze.NET.Commons.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.NET.Commons.Data
{
    public class Repository<TEntity, TId> : IRepository<TEntity, TId>, IDisposable
        where TEntity : EntityRoot where TId : EntityId
    {
        private readonly DbSet<TEntity> _dbSet;

        protected readonly DbContext Context;

        public IUnitOfWork UnitOfWork { get; set; }

        protected Repository(DbContext context, IUnitOfWork unitOfWork)
        {
            _dbSet = context.Set<TEntity>();
            Context = context;
            UnitOfWork = unitOfWork;
        }

        public async Task AddAsync(TEntity entity)
            => await _dbSet.AddAsync(entity);

        public void Update(TEntity entity)
            => _dbSet.Update(entity);

        public void Remove(TEntity entity)
            => _dbSet.Remove(entity);

        public ValueTask<TEntity> FindAsync(TId simpleId)
            => _dbSet.FindAsync(simpleId.Id);

        public Task<TEntity> FindAsync<TFilter>(TFilter filter) where TFilter : BaseFilter<TEntity>
            => BuidQueryable(new Pagination(), filter)
                .FirstOrDefaultAsync();

        public Task<List<TEntity>> FilterAsync<TFilter>(TFilter filter) where TFilter : BaseFilter<TEntity>
            => BuidQueryable(new Pagination(), filter)
                .ToListAsync();

        public Task<List<TEntity>> FilterAsync<TFilter>(Pagination pagination, TFilter filter)
            where TFilter : BaseFilter<TEntity>
            => BuidQueryable(pagination, filter)
                .ToListAsync();

        protected Task<int> CountAsync<TFilter>(TFilter filter) where TFilter : BaseFilter<TEntity>
            => _dbSet
                .AsNoTracking()
                .Where(filter.ToQuery())
                .CountAsync();

        public async Task<Pageable<TEntity>> FilterPaginateAsync<TFilter>(Pagination pagination, TFilter filter)
            where TFilter : BaseFilter<TEntity>
        {
            int total = await CountAsync(filter);

            List<TEntity> data = await BuidQueryable(pagination, filter).ToListAsync();

            return FactoryPageable(data, total, pagination.Skip ??= 0, pagination.Take ??= 30);
        }

        private IQueryable<TEntity> BuidQueryable<TFilter>(Pagination pagination, TFilter filter)
            where TFilter : BaseFilter<TEntity>
        {
            return _dbSet
                .Where(filter.ToQuery())
                .OrderByDescending(x => x.CreatedAt)
                .ConfigureSkipTakeFromPagination(pagination);
        }

        protected Pageable<TType> FactoryPageable<TType>(IList<TType> data, int total, int skip, int take)
            where TType : class
        {
            return new()
            {
                Data = data,
                Total = total,
                Pages = total > 0 ? (int)Math.Ceiling(total / (decimal)(take)) : 0,
                CurrentPage = skip <= 0 ? 1 : skip
            };
        }

        public void Dispose()
        {
            Context.Dispose();
            //UnitOfWork.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

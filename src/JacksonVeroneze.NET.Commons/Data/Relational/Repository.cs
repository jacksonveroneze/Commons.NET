using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JacksonVeroneze.NET.Commons.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.NET.Commons.Data.Relational
{
    public abstract class Repository<TEntity, TId> : IRelationalRepository<TEntity, TId>, IDisposable
        where TEntity : EntityRoot where TId : EntityId
    {
        protected readonly DbContext Context;

        protected readonly DbSet<TEntity> DbSet;

        public IUnitOfWork UnitOfWork { get; set; }

        protected Repository(DbContext context, IRelationalUnitOfWork unitOfWork)
        {
            Context = context;
            UnitOfWork = unitOfWork;

            DbSet = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
            => await DbSet.AddAsync(entity);

        public void Update(TEntity entity)
            => DbSet.Update(entity);

        public void Remove(TEntity entity)
            => DbSet.Remove(entity);

        public ValueTask<TEntity> FindAsync(TId simpleId)
            => DbSet.FindAsync(simpleId.Id);

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

        public async Task<Pageable<TEntity>> FilterPaginateAsync<TFilter>(Pagination pagination, TFilter filter)
            where TFilter : BaseFilter<TEntity>
        {
            int total = await CountAsync(filter);

            List<TEntity> data = await BuidQueryable(pagination, filter).ToListAsync();

            return FactoryPageable(data, total, pagination.Skip ??= 0, pagination.Take ??= 30);
        }

        public Task<int> CountAsync<TFilter>(TFilter filter) where TFilter : BaseFilter<TEntity>
            => DbSet
                .AsNoTracking()
                .Where(filter.ToQuery())
                .CountAsync();

        private IQueryable<TEntity> BuidQueryable<TFilter>(Pagination pagination, TFilter filter)
            where TFilter : BaseFilter<TEntity>
        {
            return DbSet
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
            GC.SuppressFinalize(this);
        }
    }
}
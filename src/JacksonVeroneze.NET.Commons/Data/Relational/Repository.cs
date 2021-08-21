using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JacksonVeroneze.NET.Commons.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace JacksonVeroneze.NET.Commons.Data.Relational
{
    public abstract class Repository<TEntity, TId> : IRelationalRepository<TEntity, TId>, IDisposable
        where TEntity : Entity, IAggregateRoot where TId : EntityId
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

        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
            => BuidQueryable(new Pagination(), expression)
                .FirstOrDefaultAsync();

        public Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression)
            => BuidQueryable(new Pagination(), expression)
                .ToListAsync();

        public Task<List<TEntity>> FilterAsync(Pagination pagination, Expression<Func<TEntity, bool>> expression)
            => BuidQueryable(pagination, expression)
                .ToListAsync();

        public async Task<Pageable<TEntity>> FilterPaginateAsync(Pagination pagination,
            Expression<Func<TEntity, bool>> expression)

        {
            int total = await CountAsync(expression);

            List<TEntity> data = await BuidQueryable(pagination, expression).ToListAsync();

            return FactoryPageable(data, total, pagination.Skip ??= 0, pagination.Take ??= 30);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
            => DbSet
                .AsNoTracking()
                .Where(expression)
                .CountAsync();

        private IQueryable<TEntity> BuidQueryable(Pagination pagination, Expression<Func<TEntity, bool>> expression)

        {
            return DbSet
                .Where(expression)
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
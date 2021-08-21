using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JacksonVeroneze.NET.Commons.DomainObjects;
using MongoDB.Driver;

namespace JacksonVeroneze.NET.Commons.Data.Document
{
    public abstract class Repository<TEntity, TId> : IDocumentRepository<TEntity, TId>, IDisposable
        where TEntity : EntityRoot where TId : EntityId
    {
        protected readonly IMongoContext Context;

        protected readonly IMongoCollection<TEntity> DbSet;

        public IUnitOfWork UnitOfWork { get; set; }

        protected Repository(IMongoContext context, IDocumentUnitOfWork unitOfWork)
        {
            Context = context;
            UnitOfWork = unitOfWork;

            DbSet = Context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public Task AddAsync(TEntity entity)
            => Context.AddCommand(async () => await DbSet.InsertOneAsync(entity));

        public void Update(TEntity entity)
            => Context.AddCommand(async () =>
                await DbSet.ReplaceOneAsync(x => x.Id == entity.Id, entity));

        public void Remove(TEntity entity)
            => Context.AddCommand(() => DbSet.DeleteOneAsync(x => x.Id == entity.Id));

        public async ValueTask<TEntity> FindAsync(TId simpleId)
            => (await DbSet.FindAsync(x => x.Id == simpleId.Id)).FirstOrDefault();

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
            => (await DbSet.FindAsync(expression)).FirstOrDefault();

        public async Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression)
            => (await DbSet.FindAsync(expression)).ToList();

        public async Task<List<TEntity>> FilterAsync(Pagination pagination, Expression<Func<TEntity, bool>> expression)
            => (await BuidQueryable(pagination, expression)).ToList();

        public async Task<Pageable<TEntity>> FilterPaginateAsync(Pagination pagination,
            Expression<Func<TEntity, bool>> expression)

        {
            long total = await CountAsync(expression);

            List<TEntity> data = (await BuidQueryable(pagination, expression)).ToList();

            return FactoryPageable(data, Convert.ToInt32(total), pagination.Skip ??= 0, pagination.Take ??= 30);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            long total = await DbSet.CountDocumentsAsync(expression);

            return (int)total;
        }

        private Task<IAsyncCursor<TEntity>> BuidQueryable(Pagination pagination,
            Expression<Func<TEntity, bool>> expression)

        {
            return DbSet.FindAsync(expression, new FindOptions<TEntity>()
            {
                Skip = pagination.Skip,
                Limit = pagination.Take,
                Sort = Builders<TEntity>.Sort.Descending(nameof(Entity.CreatedAt))
            });
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
            => GC.SuppressFinalize(this);
    }
}
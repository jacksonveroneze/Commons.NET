using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JacksonVeroneze.NET.Commons.DomainObjects;
using MongoDB.Driver;

namespace JacksonVeroneze.NET.Commons.Data.Document
{
    public abstract class Repository<TEntity, TId> : IDocumentRepository<TEntity, TId>
        where TEntity : Entity, IAggregateRoot where TId : EntityId
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
        {
            entity.MarkAsUpdated();

            Context.AddCommand(async () =>
                await DbSet.ReplaceOneAsync(x => x.Id == entity.Id, entity));
        }

        public void Remove(TEntity entity)
        {
            entity.MarkAsDeleted();

            Context.AddCommand(async () =>
                await DbSet.ReplaceOneAsync(x => x.Id == entity.Id, entity));
        }

        public async ValueTask<TEntity> FindAsync(TId simpleId)
            => (await DbSet.FindAsync(Builders<TEntity>.Filter.Eq(x => x.Id, simpleId.Id))).FirstOrDefault();

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
            => (await DbSet.FindAsync(expression)).FirstOrDefault();

        public async Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression)
            => (await DbSet.FindAsync(expression)).ToList();

        public async Task<List<TEntity>> FilterAsync(Pagination pagination, Expression<Func<TEntity, bool>> expression)
            => (await BuidQueryable(pagination, expression)).ToList();

        public async Task<PageResult<TEntity>> FilterPaginateAsync(Pagination pagination,
            Expression<Func<TEntity, bool>> expression)

        {
            long total = await CountAsync(expression);

            List<TEntity> data = (await BuidQueryable(pagination, expression)).ToList();

            return FactoryPageable(data, Convert.ToInt32(total), pagination.Page, pagination.PageSize);
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
                Skip = pagination.Page,
                Limit = pagination.PageSize,
                Sort = Builders<TEntity>.Sort.Descending(nameof(Entity.CreatedAt))
            });
        }

        protected PageResult<TType> FactoryPageable<TType>(IList<TType> data, int total, int skip, int take)
            where TType : class
        {
            return new()
            {
                Data = data,
                TotalElements = total,
                TotalPages = total > 0 ? (int)Math.Ceiling(total / (decimal)(take)) : 0,
                CurrentPage = skip <= 0 ? 1 : skip,
                PageSize = take,
            };
        }

        public void Dispose()
            => GC.SuppressFinalize(this);
    }
}
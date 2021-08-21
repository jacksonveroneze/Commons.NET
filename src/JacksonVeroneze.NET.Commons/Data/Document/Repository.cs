using System;
using System.Collections.Generic;
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

        public async Task<TEntity> FindAsync<TFilter>(TFilter filter) where TFilter : BaseFilter<TEntity>
            => (await DbSet.FindAsync(filter.ToQuery())).FirstOrDefault();

        public async Task<List<TEntity>> FilterAsync<TFilter>(TFilter filter) where TFilter : BaseFilter<TEntity>
            => (await DbSet.FindAsync(filter.ToQuery())).ToList();

        public async Task<List<TEntity>> FilterAsync<TFilter>(Pagination pagination, TFilter filter)
            where TFilter : BaseFilter<TEntity>
            => (await BuidQueryable(pagination, filter)).ToList();

        public async Task<Pageable<TEntity>> FilterPaginateAsync<TFilter>(Pagination pagination, TFilter filter)
            where TFilter : BaseFilter<TEntity>
        {
            long total = await CountAsync(filter);

            List<TEntity> data = (await BuidQueryable(pagination, filter)).ToList();

            return FactoryPageable(data, Convert.ToInt32(total), pagination.Skip ??= 0, pagination.Take ??= 30);
        }

        public async Task<int> CountAsync<TFilter>(TFilter filter) where TFilter : BaseFilter<TEntity>
        {
            long total = await DbSet.CountDocumentsAsync(filter.ToQuery());

            return (int)total;
        }

        private Task<IAsyncCursor<TEntity>> BuidQueryable<TFilter>(Pagination pagination, TFilter filter)
            where TFilter : BaseFilter<TEntity>
        {
            return DbSet.FindAsync(filter.ToQuery(), new FindOptions<TEntity>()
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
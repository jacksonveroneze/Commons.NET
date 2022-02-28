using System.Threading.Tasks;

namespace JacksonVeroneze.NET.Commons.Database.Data.Document
{
    public class UnitOfWork : IDocumentUnitOfWork
    {
        private readonly IMongoContext _context;

        public UnitOfWork(IMongoContext context)
            => _context = context;

        public async Task<bool> CommitAsync()
            => await _context.SaveChanges() > 0;
    }
}
using System.Threading.Tasks;

namespace JacksonVeroneze.NET.Commons.Data.Document
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoContext _context;

        public UnitOfWork(IMongoContext context)
            => _context = context;

        public async Task<bool> CommitAsync()
            => await _context.SaveChanges() > 0;
    }
}
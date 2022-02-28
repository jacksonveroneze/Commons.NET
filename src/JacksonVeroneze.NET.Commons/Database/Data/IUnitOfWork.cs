using System.Threading.Tasks;

namespace JacksonVeroneze.NET.Commons.Database.Data
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}

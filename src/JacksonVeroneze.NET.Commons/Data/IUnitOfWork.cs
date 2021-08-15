using System.Threading.Tasks;

namespace JacksonVeroneze.NET.Commons.Data
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}

using System;
using System.Threading.Tasks;

namespace JacksonVeroneze.NET.Commons.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> CommitAsync();
    }
}

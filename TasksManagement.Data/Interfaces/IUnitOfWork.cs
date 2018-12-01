using System;
using System.Threading.Tasks;

namespace TasksManagement.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Data.Entities.Task> GetTaksRepository();

        void SaveChanges();
        Task SaveChangesAsync();
    }
}

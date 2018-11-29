using System;
using TasksManagement.Core.Entities;

namespace TasksManagement.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> GetUserRepository();
        IRepository<Role> GetRoleRepository();
        IRepository<Task> GetTaksRepository();

        void SaveChanges();
        System.Threading.Tasks.Task SaveChangesAsync();
    }
}

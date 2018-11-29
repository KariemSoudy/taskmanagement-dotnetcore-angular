using System;
using TasksManagement.Core.Entities;
using TasksManagement.Core.Interfaces;

namespace TasksManagement.Data
{
    public class TasksUnitOfWork : IUnitOfWork
    {
        private TaskManagementDBContext _context = new TaskManagementDBContext();

        private IRepository<User> _userRepository;
        private IRepository<Role> _roleRepository;
        private IRepository<Task> _taskRepository;

        public IRepository<User> GetUserRepository()
        {
            if (this._userRepository == null) this._userRepository = new Repository<User>(_context);
            return _userRepository;
        }

        public IRepository<Role> GetRoleRepository()
        {
            if (this._roleRepository == null) this._roleRepository = new Repository<Role>(_context);
            return _roleRepository;
        }

        public IRepository<Task> GetTaksRepository()
        {
            if (this._taskRepository == null) this._taskRepository = new Repository<Task>(_context);
            return _taskRepository;
        }

        public void SaveChanges() => _context.SaveChangesAsync();

        public System.Threading.Tasks.Task SaveChangesAsync() => _context.SaveChangesAsync();




        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing) _context.Dispose();
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
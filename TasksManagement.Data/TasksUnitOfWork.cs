using System;
using System.Threading.Tasks;
using TasksManagement.Data.Interfaces;

namespace TasksManagement.Data
{
    public class TasksUnitOfWork : IUnitOfWork
    {
        private TaskManagementDBContext _context = new TaskManagementDBContext();

        private IRepository<Data.Entities.Task> _taskRepository;


        public IRepository<Data.Entities.Task> GetTaksRepository()
        {
            if (this._taskRepository == null) this._taskRepository = new Repository<Data.Entities.Task>(_context);
            return _taskRepository;
        }

        public void SaveChanges() => _context.SaveChangesAsync();

        public Task SaveChangesAsync() => _context.SaveChangesAsync();




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
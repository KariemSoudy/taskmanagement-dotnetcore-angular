using System.Collections.Generic;

namespace TasksManagement.Core.Interfaces
{
    public interface IRepository<T>
    {
        bool Add(T entity);
        bool Edit(T entity);
        bool Delete(int entityID);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetByID();
    }
}

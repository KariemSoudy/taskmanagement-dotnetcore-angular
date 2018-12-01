using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TasksManagement.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        bool Add(T entity);
        bool Edit(T entity);
        bool Delete(Expression<Func<T, bool>> predicate);

        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);

        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate);
    }
}

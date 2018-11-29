using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TasksManagement.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        bool Add(T entity);
        bool Edit(T entity);
        bool Delete(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
    }
}

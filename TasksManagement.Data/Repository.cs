using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TasksManagement.Core.Interfaces;

namespace TasksManagement.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TaskManagementDBContext _context;
        private DbSet<T> dbSet;

        public Repository(TaskManagementDBContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public bool Add(T entity)
        {
            try
            {
                _context.Add(entity);
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Delete(Expression<Func<T, bool>> predicate)
        {
            try
            {
                _context.RemoveRange(_context.Set<T>().Where(predicate));
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Edit(T entity)
        {
            try
            {
                dbSet.Attach(entity);
                var entry = _context.Entry(entity);
                entry.State = EntityState.Modified;
            }
            catch
            {
                return false;
            }

            return true;
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.AsEnumerable<T>();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).AsEnumerable<T>();
        }
    }
}

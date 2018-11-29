using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TasksManagement.Core.Entities;
using TasksManagement.Core.Interfaces;

namespace TasksManagement.Data
{
    public class TaskRepository : IRepository<Task>
    {
        private readonly TaskManagementDBContext _context;

        public TaskRepository(TaskManagementDBContext context) => _context = context;

        public bool Add(Task entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Expression<Func<Task, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public bool Edit(Task entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Task> Get(Expression<Func<Task, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Task> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}

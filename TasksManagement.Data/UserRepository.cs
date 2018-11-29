using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TasksManagement.Core.Entities;
using TasksManagement.Core.Interfaces;

namespace TasksManagement.Data
{
    public class UserRepository : IRepository<User>
    {
        private readonly TaskManagementDBContext _context;

        public UserRepository(TaskManagementDBContext context) => _context = context;

        public bool Add(User entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public bool Edit(User entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> Get(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}

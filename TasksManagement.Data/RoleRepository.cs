using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TasksManagement.Core.Entities;
using TasksManagement.Core.Interfaces;

namespace TasksManagement.Data
{
    public class RoleRepository : IRepository<Role>
    {
        private readonly TaskManagementDBContext _context;

        public RoleRepository(TaskManagementDBContext context) => _context = context;

        public bool Add(Role entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Expression<Func<Role, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public bool Edit(Role entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Role> Get(Expression<Func<Role, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Role> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}

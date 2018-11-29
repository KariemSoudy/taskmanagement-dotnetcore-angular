using System.Collections.Generic;

namespace TasksManagement.Core.Entities
{
    public class Role
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}

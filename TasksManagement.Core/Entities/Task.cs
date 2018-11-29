using System;
using System.Collections.Generic;

namespace TasksManagement.Core.Entities
{
    public class Task
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int OwnerUserID { get; set; }
        public User OwnerUser { get; set; }
        public DateTime Created { get; set; }

        public ICollection<User> AssignedTo { get; set; }
    }
}

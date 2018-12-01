using System;

namespace TasksManagement.Models
{
    public class TaskModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public UserModel OwnerUser { get; set; }
        public UserModel AssignedToUser { get; set; }
    }
}

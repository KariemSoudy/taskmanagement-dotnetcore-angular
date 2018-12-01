using System.Collections.Generic;

namespace TasksManagement.Models
{
    public class UserModel
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public List<TaskModel> OwnedTasks { get; set; }
        public List<TaskModel> AssignedTasks { get; set; }

    }
}

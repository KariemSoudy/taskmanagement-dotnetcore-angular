using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TasksManagement.Core.Entities
{
    public class User
    {
        public User()
        {
            this.OwnedTasks = new HashSet<Task>();
            this.AssignedTasks = new HashSet<Task>();
        }

        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int RoleID { get; set; }

        [Required]
        public Role Role { get; set; }

        public ICollection<Task> OwnedTasks { get; set; }
        public ICollection<Task> AssignedTasks { get; set; }
    }
}

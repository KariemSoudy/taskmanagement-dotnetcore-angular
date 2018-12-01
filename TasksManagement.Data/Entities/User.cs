using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TasksManagement.Data.Entities
{
    public class User : IdentityUser
    {
        public User()
        {
            this.OwnedTasks = new HashSet<Task>();
            this.AssignedTasks = new HashSet<Task>();
        }

        public ICollection<Task> OwnedTasks { get; set; }
        public ICollection<Task> AssignedTasks { get; set; }
    }
}

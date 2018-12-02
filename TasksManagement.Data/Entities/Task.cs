using System;
using System.ComponentModel.DataAnnotations;

namespace TasksManagement.Data.Entities
{
    public class Task
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public string OwnerUserID { get; set; }

        public string AssignedToUserID { get; set; }

        [Required]
        public User OwnerUser { get; set; }

        public User AssignedToUser { get; set; }

        public bool Completed { get; set; }
    }
}

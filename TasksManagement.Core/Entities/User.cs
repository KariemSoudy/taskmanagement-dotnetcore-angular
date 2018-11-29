namespace TasksManagement.Core.Entities
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
    }
}

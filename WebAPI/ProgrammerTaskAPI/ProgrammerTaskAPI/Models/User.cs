namespace ProgrammerTaskAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Guid RoleId { get; set; }
       // public Role UserRole { get; set; }  

      //  public List<WorkTask> Tasks { get; set; }

    }
}

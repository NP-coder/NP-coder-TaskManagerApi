using Microsoft.EntityFrameworkCore;
using ProgrammerTaskAPI.Models;

namespace ProgrammerTaskAPI.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<WorkTask> Tasks { get; set; }
    }
}

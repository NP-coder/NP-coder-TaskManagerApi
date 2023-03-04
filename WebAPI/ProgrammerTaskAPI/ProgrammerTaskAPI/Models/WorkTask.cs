using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProgrammerTaskAPI.Models
{
    public class WorkTask
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TimeSpent { get; set; }
        public string Date { get; set; }

        public Guid UserId { get; set; }
        //public User UserCreate { get; set; }
    }
}

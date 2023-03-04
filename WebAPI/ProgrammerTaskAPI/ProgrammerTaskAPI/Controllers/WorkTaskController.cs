using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgrammerTaskAPI.Data;
using ProgrammerTaskAPI.Models;
using System.Linq;
using System.Security.Claims;

namespace ProgrammerTaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkTaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public WorkTaskController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        [HttpGet]
        [Route("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _context.Tasks.ToListAsync();

            return Ok(tasks);
        }

        
        [HttpGet]
        [Route("byUser")]
        [Authorize(Roles = "Programmer")]
        public async Task<IActionResult> GetTaskByUser()
        {
            var tasks = await _context.Tasks.Where(t => t.UserId.Equals(UserId)).ToListAsync();
            
            return Ok(tasks);
        }

        
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetTaskById([FromRoute] Guid id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if(task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPost]
        [Authorize(Roles = "Programmer")]
        public async Task<IActionResult> AddTask([FromBody] WorkTask taskRequest)
        {

            taskRequest.Id = Guid.NewGuid();
            taskRequest.UserId = UserId;
            //taskRequest.UserId = Guid.Parse("3541CA0C-AC77-4A54-B642-F1AF8E7602FF");

            await _context.Tasks.AddAsync(taskRequest);
            await _context.SaveChangesAsync();
            return Ok(taskRequest);
        }
        
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateTask([FromRoute] Guid id, WorkTask taskRequest)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            task.Name = taskRequest.Name;
            task.TimeSpent = taskRequest.TimeSpent;
            task.Date = taskRequest.Date;

            await _context.SaveChangesAsync();

            return Ok(task);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}

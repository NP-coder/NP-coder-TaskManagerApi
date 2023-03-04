using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgrammerTaskAPI.Data;
using ProgrammerTaskAPI.Models;

namespace ProgrammerTaskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize (Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
           var users = await _context.Users.ToListAsync();

            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User userRequest)
        {
            var userRoles = _context.Roles.Where(role => role.Name.Equals("Programmer"));

            userRequest.Id = Guid.NewGuid();
            userRequest.RoleId = userRoles.First().Id;

            await _context.Users.AddAsync(userRequest);
            await _context.SaveChangesAsync();
            return Ok(userRequest);
        }

    }
}

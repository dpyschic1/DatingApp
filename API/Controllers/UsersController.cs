using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize] //enforce authorization for whole class

    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
        
        public UsersController(DataContext context)
        {
            _context = context;
            
        }


        [AllowAnonymous] //bypass authorization
        //To handle get request for getting all users from the Users table
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }   

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    //Assign api controller attribute to access http api resonses
    [ApiController]
    //Assign a route( for the url) to access the controller, in this case using placeholder text, which will be
    //replaced during runtime with the word "Users" i.e. /api/Users
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        
        public UsersController(DataContext context)
        {
            _context = context;
            
        }



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
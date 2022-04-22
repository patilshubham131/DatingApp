using Microsoft.AspNetCore.Mvc;
using API.Data;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;

namespace API.Controllers
{
    //these are removed because they are decalred in the base api class
    // [ApiController]
    // [Route("api/[controller]")] 

    [Authorize]
    public class UsersController : BaseApiController
    {
       // private readonly DataContext context;
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
           // this.context = context;
        }

        // public UsersController(DataContext context)
        // {
        //     this.context = context;
        // }

        [HttpGet]
        // [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
           // return await context.Users.ToListAsync();
           var users = await userRepository.GetUsersAsync();
           return Ok(users);
        }

        [HttpGet("{username}")]
        // [AllowAnonymous]
        public async Task<ActionResult<AppUser>> GetUser(string username)
        {
            //return context.Users.Find(id);
            return await userRepository.GetUserByUserName(username);
        }
    }
}
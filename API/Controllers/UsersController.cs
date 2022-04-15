using Microsoft.AspNetCore.Mvc;
using API.Data;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    //these are removed because they are decalred in the base api class
    // [ApiController]
    // [Route("api/[controller]")] 
    public class UsersController : BaseApiController
    {
        private readonly DataContext context;
        public UsersController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task< ActionResult<IEnumerable<AppUser>>> GetUsers(){
            return await context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<AppUser> GetUsers(int id){
            return context.Users.Find(id);
        }
    }
}
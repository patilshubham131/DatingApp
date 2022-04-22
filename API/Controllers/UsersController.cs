using Microsoft.AspNetCore.Mvc;
using API.Data;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using API.DTOs;
using AutoMapper;

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
        public IMapper Mapper { get; set; }

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this.Mapper = mapper;
            this.userRepository = userRepository;
            // this.context = context;
        }

        // public UsersController(DataContext context)
        // {
        //     this.context = context;
        // }

        [HttpGet]
        // [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            // return await context.Users.ToListAsync();
            var users = await userRepository.GetMembersAsync();

            var usersToReturn = Mapper.Map<IEnumerable<MemberDto>>(users);
           return Ok(usersToReturn);
        }

        [HttpGet("{username}")]
        // [AllowAnonymous]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            //return context.Users.Find(id);
            var user = await userRepository.GetMemberAsync(username);
            return Mapper.Map<MemberDto>(user);
        }
    }
}
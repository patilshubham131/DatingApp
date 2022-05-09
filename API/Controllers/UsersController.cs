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
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using API.Extensions;

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
        public IPhotoService PhotoService { get; set; }

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            this.PhotoService = photoService;
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

        [HttpGet("{username}", Name="GetUser")]
        // [AllowAnonymous]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            //return context.Users.Find(id);
            var user = await userRepository.GetMemberAsync(username);
            return Mapper.Map<MemberDto>(user);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {

            // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.GetUsername();

            var user = await userRepository.GetUserByUserName(username);

            Mapper.Map(memberUpdateDto, user);

            userRepository.Update(user);

            if (await userRepository.SaveAllAsync())
                return NoContent();

            return BadRequest("failure while updating user.");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {

            var newuser = await userRepository.GetUserByUserName(User.GetUsername());

            var result = await PhotoService.AddPhotoAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(newuser.Photos.Count == 0){
                photo.IsMain = true;
            }

            newuser.Photos.Add(photo);

            if(await userRepository.SaveAllAsync()){
               // return Mapper.Map<PhotoDto>(photo);

               return CreatedAtRoute("GetUser", new {username = newuser.UserName},Mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("photo upload failed");
        }
    }
}
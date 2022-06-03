using Microsoft.AspNetCore.Identity;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using API.Services;
using API.DTOs;
using AutoMapper;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        // private readonly DataContext context;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
         ITokenService tokenService, IMapper mapper)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.mapper = mapper;
            this.tokenService = tokenService;
            // this.context = context;

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto newUser)
        {
            var isExistingUser = await ExistingUser(newUser.UserName);

            var user = mapper.Map<AppUser>(newUser);

            if (isExistingUser)
            {
                return BadRequest("User name is taken");
            }

            // using var hmac = new HMACSHA512();

            // var user = new AppUser()
            // {
            //     UserName = newUser.UserName.ToLower(),
            //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newUser.Password)),
            //     PasswordSalt = hmac.Key
            // };

            user.UserName = newUser.UserName.ToLower();
            // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newUser.Password));
            // user.PasswordSalt = hmac.Key;

            // context.Users.Add(user);

            // await context.SaveChangesAsync();

            var result = await userManager.CreateAsync(user, newUser.Password);

            if(!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await userManager.AddToRoleAsync(user, "Member");
            if(!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);

            return new UserDto
            {
                UserName = user.UserName,
                Token = await tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        private async Task<bool> ExistingUser(string username)
        {

            return await userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto user)
        {

            var tempuser = userManager.Users.Include(p => p.Photos).SingleOrDefault(x => x.UserName == user.UserName.ToLower());

            if (tempuser == null)
            {
                return Unauthorized("Invalid Username");
            }

            var result = await signInManager.CheckPasswordSignInAsync(tempuser, user.Password, false);
            
            if(!result.Succeeded) return Unauthorized();

            //commented as a part of identity implementation.
            // using var hmac = new HMACSHA512(tempuser.PasswordSalt);

            // var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));

            // for (int i = 0; i < computedHash.Length; i++)
            // {
            //     if (computedHash[i] != tempuser.PasswordHash[i])
            //     {
            //         return Unauthorized("Invalid Password");
            //     }
            // }

            return new UserDto
            {
                UserName = tempuser.UserName,
                Token = await tokenService.CreateToken(tempuser),
                PhotoUrl = tempuser.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = tempuser.KnownAs,
                Gender = tempuser.Gender
            };
        }

    }
}
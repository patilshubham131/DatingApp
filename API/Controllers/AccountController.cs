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

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            this.tokenService = tokenService;
            this.context = context;

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto newUser)
        {
            var isExistingUser = await ExistingUser(newUser.UserName);

            if (isExistingUser)
            {
                return BadRequest("User name is taken");
            }

            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
                UserName = newUser.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newUser.Password)),
                PasswordSalt = hmac.Key
            };
            context.Users.Add(user);

            await context.SaveChangesAsync();

            return new UserDto{
                UserName = user.UserName,
                Token = tokenService.CreateToken(user)
            };
        }

        private async Task<bool> ExistingUser(string username)
        {

            return await context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto user)
        {

            var tempuser = context.Users.Include(p=> p.Photos).SingleOrDefault(x => x.UserName == user.UserName);

            if (tempuser == null)
            {
                return Unauthorized("Invalid Username");
            }

            using var hmac = new HMACSHA512(tempuser.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != tempuser.PasswordHash[i])
                {
                    return Unauthorized("Invalid Password");
                }
            }

           return new UserDto{
                UserName = tempuser.UserName,
                Token = tokenService.CreateToken(tempuser),
                PhotoUrl = tempuser.Photos.FirstOrDefault(x=> x.IsMain)?.Url
            };
        }

    }
}
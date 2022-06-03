using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using API.Entities;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {

            var users = await userManager.Users
            .Include(r=> r.UserRoles)
            .ThenInclude(r=> r.Role)
            .OrderBy(u=> u.UserName)
            .Select(u=> new {
                    u.Id,
                    UserName = u.UserName,
                    Roles = u.UserRoles.Select(r=>r.Role.Name).ToList()
            })
            .ToListAsync();
            return Ok(users);
        }

        [Authorize(Policy = "ModerateAdminRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetphotoForModeration()
        {
            return Ok("only admin or moderator");
        }

        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles){

            var selectedRoles = roles.Split(",").ToArray();

            var user = await userManager.FindByNameAsync(username);

            var userroles = await userManager.GetRolesAsync(user);

            var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userroles));

            if(!result.Succeeded) return BadRequest("failed to add roles");

            result = await userManager.RemoveFromRolesAsync(user, userroles.Except(selectedRoles));

            if(!result.Succeeded) return BadRequest("failed to remove from roles");
            
            return Ok(await userManager.GetRolesAsync(user));
        }
    }
}
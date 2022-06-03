using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Collections.Generic;
using API.Entities;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager){

            if(await userManager.Users.AnyAsync()) return ;

            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            var roles = new List<AppRole>(){

                new AppRole{Name = "Member"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Moderator"}
            };

            foreach(var role in roles){
                await roleManager.CreateAsync(role);
            }

            foreach(var user in users){
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("1234"));
                // user.PasswordSalt = hmac.Key;
                
                await userManager.CreateAsync(user,"@Bcd123");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser{
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "@Bcd123");
            await userManager.AddToRolesAsync(admin, new[]{"Admin", "Moderator"});

            // await context.SaveChangesAsync();
        }
    }
}
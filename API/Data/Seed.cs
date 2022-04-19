using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Collections.Generic;
using API.Entities;
using System.Security.Cryptography;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context){

            if(await context.Users.AnyAsync()) return ;

            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach(var user in users){
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("1234"));
                user.PasswordSalt = hmac.Key;
                
                context.Users.Add(user);
            }

            await context.SaveChangesAsync();
        }
    }
}
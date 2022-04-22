using System.Threading.Tasks;
using System.Collections.Generic;
using API.Entities;
using API.Data;
using API.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        public UserRepository(DataContext context)
        {
            this.context = context;
        }


        public async Task<AppUser> GetUserByIdAsync(int id){
            return await context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserName(string username){
            return await context.Users
            .Include(p=> p.Photos)
            .SingleOrDefaultAsync(x=> x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync(){
            return await context.Users
            .Include(p=> p.Photos)
            .ToListAsync();
        }

        public async Task<bool> SaveAllAsync(){
            return await context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user){
            context.Entry(user).State = EntityState.Modified;
        }
    }
}
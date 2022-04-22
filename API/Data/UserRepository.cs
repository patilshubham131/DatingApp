using System.Threading.Tasks;
using System.Collections.Generic;
using API.Entities;
using API.Data;
using API.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        public IMapper Mapper { get; set; }
        public UserRepository(DataContext context, IMapper mapper)
        {
            this.Mapper = mapper;
            this.context = context;
        }


        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserName(string username)
        {
            return await context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.Users
            .Include(p => p.Photos)
            .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            context.Entry(user).State = EntityState.Modified;
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {

            return await context.Users
            .ProjectTo<MemberDto>(Mapper.ConfigurationProvider)
            .ToListAsync();
        }
         public async Task<MemberDto> GetMemberAsync(string username)
        {

            return await context.Users
            .Where(x=> x.UserName == username)
            .ProjectTo<MemberDto>(Mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }
    }
}
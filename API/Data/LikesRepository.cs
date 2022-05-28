using API.Interfaces;
using System.Threading.Tasks;
using API.Entities;
using System.Collections.Generic;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using API.Extensions;
using API.Helpers;
namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext context;
        public LikesRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await context.Likes.FindAsync(sourceUserId, likedUserId);
        }
        public async Task<AppUser> GetuserWithLikes(int UserId)
        {
                return await context.Users
                .Include(x=>x.LikedUser)
                .FirstOrDefaultAsync(x=> x.Id == UserId);
        }
        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = context.Likes.AsQueryable();

            if( likesParams.Predicate == "liked"){
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.LikedUser);
            }

            if(likesParams.Predicate == "likedBy"){
                likes = likes.Where(like => like.LikedUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            // return await users.Select(user => new LikeDto{
            //     UserName = user.UserName,
            //     KnownAs  = user.KnownAs,
            //     Age = user.DateOfBirth.CalculateAge(),
            //     PhotoUrl = user.Photos.FirstOrDefault(p=> p.IsMain).Url,
            //     City = user.City,
            //     Id = user.Id
            // }).ToListAsync();

            var likedUsers= users.Select(user => new LikeDto{
                UserName = user.UserName,
                KnownAs  = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p=> p.IsMain).Url,
                City = user.City,
                Id = user.Id
            });

            return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber
            ,likesParams.PageSize);
        }
    }
}
using System.Threading.Tasks;
using API.Entities;
using System.Collections.Generic;
using API.DTOs;
using API.Helpers;
namespace API.Interfaces
{
    public interface ILikesRepository
    {
         Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
         Task<AppUser> GetuserWithLikes(int UserId);
         Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
         
         
    }
}
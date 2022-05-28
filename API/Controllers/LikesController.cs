using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using API.Extensions;
using API.Entities;
using System.Collections.Generic;
using API.DTOs;
using API.Helpers;
namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository userRepository;
        private readonly ILikesRepository likesRepository;
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            this.likesRepository = likesRepository;
            this.userRepository = userRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username){
            var sourceUserId = User.GetUserId();
            var likedUser = await userRepository.GetUserByUserName(username);
            var sourceUser = await likesRepository.GetuserWithLikes(sourceUserId);

            if(likedUser == null){
                return NotFound();
            }

            if(sourceUser.UserName == username){
                return BadRequest("you can't like yourself");
            }

            var userlike = await likesRepository.GetUserLike(sourceUserId, likedUser.Id);

            if(userlike != null){
                return BadRequest("you already like this user");
            }

            var userLike = new UserLike{
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUser.Add(userLike);

            if(await userRepository.SaveAllAsync()) return Ok();

            return BadRequest("failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams){
            
            likesParams.UserId = User.GetUserId();
            var users = await likesRepository.GetUserLikes(likesParams);
            Response.AddPaginationHeader(users.CurrenctPage, users.PageSize, users.TotalCount, users.TotalPages);
            
            return Ok(users);
        }
    }
}
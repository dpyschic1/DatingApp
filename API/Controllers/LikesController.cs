using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public LikesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId(); //get the current user id
            var likeUser = await _uow.UserRepository.GetUserByUsernameAsync(username); // get the user from repository who the current user wants to like
            var sourceUser = await _uow.LikesRepository.GetUserWithLikes(sourceUserId); // get the source User object through current user id

            if(likeUser == null) return NotFound(); // if the user to be liked is not available

            if(sourceUser.UserName == username) return BadRequest("You cannot like yourself"); // if the user to be liked is the current user

            var userLike = await _uow.LikesRepository.GetUserLike(sourceUserId, likeUser.Id); //Get the user like object from likes repository

            if(userLike != null) return BadRequest("You already like this user");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = likeUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if( await _uow.Complete()) return Ok();

            return BadRequest("Failed to like user");
        }


        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();

            var users = await _uow.LikesRepository.GetUserLikes(likesParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }
    }
}
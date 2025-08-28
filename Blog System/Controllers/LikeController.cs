using Blog_System.Models.Entities;
using Blog_System.Servicies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog_System.Controllers
{
    public class LikeController : Controller
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLikeToPost(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingLike = await _likeService.IsPostLikedByUser(userId, postId);

            if (existingLike)
            {
                await _likeService.DeleteLikeFromPost(userId, postId);
            }
            else
            {
                await _likeService.AddLikeToPost(userId, postId);
            }


            var newCount = await _likeService.GetPostLikesCountAsync(postId);
            return Json(new { newLikeCount = newCount });
        }


        //public async Task<IActionResult> ToggleLike(int postId)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    var existingLike = await _likeService.IsPostLikedByUser(userId, postId);

        //    if (existingLike)
        //    {
        //        // عمل Unlike
        //        await _likeService.DeleteLikeFromPost(userId, postId);
        //        var likeCount = await _likeService.GetPostLikesCountAsync(postId);
        //        return Json(new { success = true, liked = false, likeCount });
        //    }
        //    else
        //    {
        //        // عمل Like
        //        var like = new Like { PostId = postId, UserId = userId };
        //        await _likeService.AddLikeToPost(userId, postId);
        //        var likeCount = await _likeService.GetPostLikesCountAsync(postId);
        //        return Json(new { success = true, liked = true, likeCount });
        //    }
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLike([FromBody] dynamic data)
        {
            int postId = (int)data.postId;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingLike = await _likeService.IsPostLikedByUser(userId, postId);

            if (existingLike)
            {
                // عمل Unlike
                await _likeService.DeleteLikeFromPost(userId, postId);
                var likeCount = await _likeService.GetPostLikesCountAsync(postId);
                return Json(new { success = true, liked = false, likeCount });
            }
            else
            {
                // عمل Like
                await _likeService.AddLikeToPost(userId, postId);
                var likeCount = await _likeService.GetPostLikesCountAsync(postId);
                return Json(new { success = true, liked = true, likeCount });
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetLikeCount(int postId)
        {
            var count = await _likeService.GetPostLikesCountAsync(postId);

            return Ok(count);
        }



    }
}

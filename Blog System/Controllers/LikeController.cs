using Blog_System.Models.Entities;
using Blog_System.Servicies;
using Microsoft.AspNetCore.Http.HttpResults;
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddLikeToPost(int postId)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    var existingLike = await _likeService.IsPostLikedByUser(userId, postId);

        //    if (existingLike != null)
        //    {
        //        await _likeService.DeleteLikeFromPost(userId, postId);
        //    }
        //    else
        //    {
        //        await _likeService.AddLikeToPost(userId, postId);
        //    }


        //    var newCount = await _likeService.GetPostLikesCountAsync(postId);
        //    return Json(new { newLikeCount = newCount });
        //}


        

        [HttpGet]
        public async Task<IActionResult> GetLikeCount(int postId)
        {
            var count = await _likeService.GetPostLikesCountAsync(postId);

            return Ok(count);
        }



    }
}

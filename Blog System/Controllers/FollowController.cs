using Blog_System.Models.Data;
using Blog_System.Models.Entities;
using Blog_System.Servicies;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog_System.Controllers
{
    public class FollowController : Controller
    {
        private readonly UserManager<UserApplication> _userManager;
        private readonly IFollowService _followService;

        public FollowController(UserManager<UserApplication> userManager, IFollowService followService)
        {
            _userManager = userManager;
            _followService = followService;
        }
        // Follower اللي مسجل دخول هو ال 

        public async Task<IActionResult> AddFollow(string targetUserId) // targetUserId => FollwingId
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // FollowerId

            await _followService.AddAsync(userId, targetUserId);
            return RedirectToAction("Profile", "Profile", new {id = targetUserId});
        }

        public async Task<IActionResult> UnFollow(string followingId)
        {
            var followerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _followService.Delete(followerId, followingId);

            await _followService.IsFollowingAsync(followerId, followingId);

            return RedirectToAction("Profile", "Profile", new { id = followingId });
        }

        // Followings, Followers في صفحه ال Follow and unfollow عملت بفضل الله طريقتين لعرض زرار  

        // View Model الطريقه الاولي هنا وهي ان استخدمت ال 
        public async Task<IActionResult> GetFollowers(string id)
        {
            var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

           var followers =  await _followService.GetFollwersAsync(id);

            var result = new List<FollowStatusViewModel>();

            foreach (var follower in followers)
            {
                var isFollowing = await _followService.IsFollowingAsync(CurrentUserId, follower.Id);

                result.Add(new FollowStatusViewModel
                {
                    Id = follower.Id,
                    Image = follower.Image,
                    IsFollowing = isFollowing,
                    UserName = follower.UserName
                });
            }

            return View(result);
        }

        // View الطريقه التانيه هنا وهي في ال 
        public async Task<IActionResult> GetFollowings(string userId)
        {
            var followings = await _followService.GetFollwingsAsync(userId);

            return View(followings);
        }


    }
}

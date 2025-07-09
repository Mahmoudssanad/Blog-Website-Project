using Blog_System.Models.Data;
using Blog_System.Models.Entities;
using Blog_System.Servicies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog_System.Controllers
{
    public class FollowController : Controller
    {
        private readonly UserManager<UserApplication> _userManager;
        private readonly INotificationService _notificationService;

        public FollowController(UserManager<UserApplication> userManager, INotificationService notificationService)
        {
            _userManager = userManager;
            _notificationService = notificationService;
        }
        public async Task<IActionResult> FollowUser(string targetUserId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var notification = new Notification
            {
                UserId = targetUserId,
                Title = "New Follow",
                Content = $"{currentUser.UserName} Follow you",
                SenderName = $"{currentUser.UserName}",
                Type = "Follow"
            };

            //await _notificationService.CreateNotificationAsync(targetUserId, "New Follow",
            //    $"{currentUser.UserName} Follow you", "Follow",$"{currentUser.UserName}", true);

            return RedirectToAction("Profile", "Users", new { id = targetUserId });
        }
    }
}

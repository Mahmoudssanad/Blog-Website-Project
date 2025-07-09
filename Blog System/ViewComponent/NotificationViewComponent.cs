using Blog_System.Models.Entities;
using Blog_System.Servicies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog_System.ViewComponents
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<UserApplication> _userManager;

        public NotificationViewComponent(INotificationService notificationService, UserManager<UserApplication> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return View(new List<Notification>());
            }

            var notifications = await _notificationService.GetUserNotificationsAsync(user.Id);
            return View(notifications);
        }
    }
}

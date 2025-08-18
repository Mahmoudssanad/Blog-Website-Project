using Blog_System.Models.Data;
using Blog_System.Servicies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog_System.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly AppDbContext _context;

        public NotificationController(INotificationService notificationService, AppDbContext context)
        {
            _notificationService = notificationService;
            _context = context;
        }

        public async Task<IActionResult> Read(int id)
        {
            var notification = await _notificationService.MarkAsReadAsync(id);

            if (!string.IsNullOrEmpty(notification?.RedirectUrl))
            {
                return Redirect(notification.RedirectUrl);
            }

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Refresh()
        {
            return ViewComponent("Notification");
        }
    }
}

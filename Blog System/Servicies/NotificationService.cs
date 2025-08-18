using Blog_System.Models.Data;
using Blog_System.Models.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Blog_System.Hubs; // هنحتاجه
using System.Threading.Tasks;

namespace Blog_System.Servicies
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(AppDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task CreateNotificationAsync(Notification notification0)
        {
            var notification = new Notification
            {
                UserId = notification0.UserId,
                CreatedAt = DateTime.UtcNow,
                Title = notification0.Title,
                Content = notification0.Content,
                Type = notification0.Type,
                SenderName = notification0.SenderName,
                RedirectUrl = notification0.RedirectUrl,
                IsRead = notification0.IsRead
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // إرسال الإشعار للمستخدم لو أونلاين
            await _hubContext.Clients.User(notification.UserId)
                .SendAsync("ReceiveNotification", notification.Content, notification.RedirectUrl);
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(string userId)
        {
            return await _context.Notifications
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var notifications = await _context.Notifications
                .Where(x => x.UserId == userId && !x.IsRead)
                .ToListAsync();

            if (notifications != null && notifications.Count > 0)
            {
                foreach (var notification in notifications)
                {
                    notification.IsRead = true;
                }
                _context.UpdateRange(notifications);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Notification?> MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);

            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                _context.Update(notification);
                await _context.SaveChangesAsync();
            }

            return notification; // رجعه بعد التعديل
        }



    }
}

using Blog_System.Models.Data;
using Blog_System.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog_System.Servicies
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
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
                // User ازي دي هتتباصا من ال 
                IsRead = notification0.IsRead
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(string userId)
        {
            return await _context.Notifications.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var notifications = await _context.Notifications.Where(x => x.UserId == userId && !x.IsRead).ToListAsync();

            if (notifications != null)
            {
                foreach (var notification in notifications)
                {
                    notification.IsRead = true;
                }
                _context.Update(notifications);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);

            if(notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                _context.Update(notification);
                await _context.SaveChangesAsync();
            }
        }
    }
}

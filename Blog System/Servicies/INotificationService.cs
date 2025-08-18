using Blog_System.Models.Entities;

namespace Blog_System.Servicies
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(Notification notification);

        Task<List<Notification>> GetUserNotificationsAsync(string userId);

        // جعل الاشعار ك مقروء
        Task<Notification> MarkAsReadAsync(int notificationId);

        // جعل كل الاشعارات كمقروءه
        Task MarkAllAsReadAsync(string userId);
    }
}

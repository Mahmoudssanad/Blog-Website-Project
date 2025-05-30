using Microsoft.AspNetCore.Identity;

namespace Blog_System.Models.Entities
{
    public class UserApplication : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? Image { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Friend> Friends { get; set; } = new List<Friend>();
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Like> Likes { get; set; } = new List<Like>();
        public List<Notification> Notifications { get; set; } = new();
    }
}

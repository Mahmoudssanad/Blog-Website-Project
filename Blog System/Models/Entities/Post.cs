using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_System.Models.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ImageURL { get; set; }
        public DateTime PublichDate { get; set; }
        public bool Visible { get; set; }

        [ForeignKey("UserApplication")]
        public string UserId { get; set; }
        public UserApplication UserApplication { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Like> Likes { get; set; } = new List<Like>();
        public List<Notification> Notifications { get; set; } = new();
    }
}

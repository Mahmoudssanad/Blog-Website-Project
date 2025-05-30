using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_System.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEdited { get; set; }
        public int ParentCommentId { get; set; }

        [ForeignKey("UserApplication")]
        public string UserId { get; set; }
        public UserApplication UserApplication { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }

        public List<Like> Likes { get; set; } = new();
    }
}

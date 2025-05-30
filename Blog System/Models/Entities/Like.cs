using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_System.Models.Entities
{
    public class Like
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserApplication")]
        public string UserId { get; set; }
        public UserApplication UserApplication { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [ForeignKey("Comment")]
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}

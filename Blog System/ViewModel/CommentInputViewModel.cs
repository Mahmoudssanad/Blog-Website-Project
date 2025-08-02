using System.ComponentModel.DataAnnotations;

namespace Blog_System.ViewModel
{
    public class CommentInputViewModel
    {
        [Required]
        public string Content { get; set; }

        public int PostId { get; set; }

        public int? ParentCommentId { get; set; }
    }
}

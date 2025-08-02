using Blog_System.Models.Entities;

namespace Blog_System.ViewModel
{
    public class PostDetailsViewModel
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
        public Comment NewComment { get; set; } = new Comment();// علشان الفورم
    }
}

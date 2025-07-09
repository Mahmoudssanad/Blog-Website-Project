using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_System.Models.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        // ===>
        public string Type { get; set; }
        public string SenderName { get; set; }

        public string? RedirectUrl { get; set; }


        [ForeignKey("UserApplication")]
        public string UserId { get; set; }
        public UserApplication UserApplication { get; set; }

        [ForeignKey("Post")]
        public int? PostId { get; set; }
        public Post Post { get; set; }

        // Polymorphic ===> ؟؟Notification Foriegn key لازم اعمل لكل حاجه عاوزه ترتبط بال 
    }
}

using System.ComponentModel.DataAnnotations;

namespace Blog_System.ViewModel
{
    public class PostViewModel
    {
        public string? Title { get; set; }

        public string? Content { get; set; }

        public IFormFile? ImageFile { get; set; } // صورة البوست

        public bool Visible { get; set; } = true;
    }
}

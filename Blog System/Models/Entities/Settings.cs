using Blog_System.Models.Enums;

namespace Blog_System.Models.Entities
{
    public class Settings
    {
        public int Id { get; set; }
        public LanguageEnum Language { get; set; }
        public bool IsDarkMode { get; set; }
    }
}

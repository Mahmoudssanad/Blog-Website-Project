using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_System.Models.Entities
{
    public class Friend
    {
        // Guid => generat unique string type, 16-byte of size
        // بالبطء Indexing مش بستخدمه لو عندي داتا بيز فيها صفوف كتيره لانه بيأثر علي ال 
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid FrindId { get; set; }

        [ForeignKey("UserApplication")]
        public string UserId { get; set; }
        public UserApplication UserApplication {  get; set; }
    }
}
